using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerState
{
    Idle,
    IsRunning,
    IsSliding,
    IsFlying,
    IsFalling,
    InPropulsion,
}

public class PlayerController : Entity
{
    [Header("Player")]
    public PlayerState MainState;
    [SerializeField] float speed;
    [SerializeField] float groundDeceleration = 10f, airDeceleration = 1f;
    [SerializeField] Vector2 walkLimits, flyLimits;

    [Header("Ground detection")]
    [SerializeField] Transform feetPoint;
    [SerializeField] float radius = 0.5f;
    [SerializeField] LayerMask groundMask;

    [Header("Fly")]
    [SerializeField] ParticleSystem fireFX;
    [SerializeField] float propulsionDelay = 0.5f, spamDelay = 0.2f;
    [SerializeField] float flyForce;
    [SerializeField] float consumeSpeed = 3f, reloadSpeed = 1.5f;
    bool flyInputPressed;
    bool freezeFlying;
    float flyInputTimer;

    [Header("Fuel")]
    [SerializeField] float reloadDelay;
    [SerializeField] float fuelCapacity = 50f;

    PlayerAnimator animator;
    float inputDirection;
    float delay;
    float fuel;
    Rigidbody2D rb;
    bool reloading;
    Dictionary<ModuleType, PlayerModule> modules = new Dictionary<ModuleType, PlayerModule>();
    Vector2 move;
    Vector2 lastGroundPosition;

    public Rigidbody2D Rigidbody { get => rb; }
    public float InputDirection { get => inputDirection; }
    public float Fuel 
    { 
        get => fuel; 
        set
        {
            if (value < 0)
                value = 0;

            if (value > fuelCapacity)
                value = fuelCapacity;

            fuel = value;
        }
    }

    private void Start()
    {
        inputDirection = 1f;
        animator = GetComponentInChildren<PlayerAnimator>();
        rb = GetComponent<Rigidbody2D>();
        Fuel = fuelCapacity;

        foreach (var item in GetComponents<PlayerModule>())
        {
            item.Initialize(this);
            modules.Add(item.myType, item);
        }
    }

    #region Modules
    public bool TryGetModule(ModuleType moduleType, out PlayerModule module)
    {
        if (modules.TryGetValue(moduleType, out module))
            return true;

        return false;
    }

    public PlayerCapacity GetCapacity(ModuleType moduleType)
    {
        return modules[moduleType] as PlayerCapacity;
    }

    public bool UseModule(ModuleType moduleType)
    {
        bool getModule = TryGetModule(moduleType, out PlayerModule module);
        if (getModule)
            module.Use();
        
        return getModule;
    }

    public void Propulsion()
    {
        bool value = GetCapacity(ModuleType.Propulsion).IsReady() && UseModule(ModuleType.Propulsion);
        if (value)
        {
            fireFX.Play();
            SetState(PlayerState.InPropulsion);
            StartCoroutine(SetStateDelayed(PlayerState.Idle, propulsionDelay));
        }
    }
    #endregion
    
    public void SetState(PlayerState newState)
    {
        MainState = newState;
        animator.SetAnimState(newState);
    }

    public IEnumerator SetStateDelayed(PlayerState newState, float delay)
    {
        yield return new WaitForSeconds(delay);
        SetState(newState);
    }

    IEnumerator ReloadFuel()
    {
        reloading = true;
        yield return new WaitForSeconds(3f);
        reloading = false;
    }

    public void FakeDeath()
    {
        ResetXVelocity();
        ResetYVelocity();
        transform.position = lastGroundPosition;
    }

    void Movements()
    {
        if (MainState == PlayerState.IsSliding)
            return;

        float xInput = Input.GetAxisRaw("Horizontal");
        if (xInput != 0)
        {
            inputDirection = Input.GetAxisRaw("Horizontal");
            Vector2 v = Vector2.right * xInput * speed;
            Rigidbody.AddForce(v);
            if (OnGround())
                SaveGroundPosition();
        }
    }

    public void Push(float force, Vector2 direction)
    {
        ResetYVelocity();
        ResetXVelocity();
        rb.AddForce(direction * force);
    }

    void ClampVelocity()
    {
        move = Rigidbody.velocity;
        move.y = Mathf.Clamp(move.y, flyLimits.x, Mathf.Infinity);
        if (MainState != PlayerState.IsSliding && Input.GetAxisRaw("Horizontal") == 0) //deceleration
        {
            if (OnGround())
            {
                move.x = Mathf.Lerp(move.x, 0, groundDeceleration * Time.fixedDeltaTime);
            }
            else
                move.x = Mathf.Lerp(move.x, 0, airDeceleration * Time.fixedDeltaTime);
        }

        if (MainState != PlayerState.IsSliding)
            move.x = Mathf.Clamp(move.x, walkLimits.x, walkLimits.y);

        Rigidbody.velocity = move;
    }

    public void FreezeFrame(float amount)
    {
        FeedbackManager.Instance.FreezeFrame(0f, amount);
    }

    public void ResetYVelocity()
    {
        Vector2 newVelocity = Rigidbody.velocity;
        newVelocity.y = 0;

        Rigidbody.velocity = newVelocity;
    }

    public void ResetXVelocity()
    {
        Vector2 newVelocity = Rigidbody.velocity;
        newVelocity.x = 0;

        Rigidbody.velocity = newVelocity;
    }

    void SlowReload()
    {
        delay += Time.deltaTime;
        if (delay > reloadDelay && !reloading)
            Fuel += reloadSpeed * Time.deltaTime;
    }

    void SaveGroundPosition()
    {
        lastGroundPosition = transform.position;
        //print(lastGroundPosition);
    }

    void Landing()
    {
        SaveGroundPosition();
    }

    void ManageState()
    {
        if (MainState == PlayerState.IsSliding || MainState == PlayerState.InPropulsion)
            return;

        if ((MainState == PlayerState.IsFalling || MainState == PlayerState.IsFlying) && OnGround())
            Landing();

        float walkThreshold = 0.2f;
        if (Rigidbody.velocity.x > walkThreshold || Rigidbody.velocity.x < -walkThreshold)
            SetState(PlayerState.IsRunning);
        else
            SetState(PlayerState.Idle);

        if (!OnGround())
        {
            if (Rigidbody.velocity.y < 0)
                SetState(PlayerState.IsFalling);
            else if (Rigidbody.velocity.y > 0)
                SetState(PlayerState.IsFlying);
        }
    }

    void ManageInputs()
    {
        if (MainState == PlayerState.IsSliding)
            return;

        foreach (var item in modules.Values)
            if (Input.GetButtonDown(item.buttonName))
                UseModule(item.myType);
    }

    void Flying()
    {
        bool correctState = MainState != PlayerState.IsSliding && MainState != PlayerState.InPropulsion;
        bool canFly = correctState && Fuel > 0 && !reloading && !freezeFlying;

        if (Input.GetAxisRaw("Vertical") <= 0 && flyInputPressed)
        {
            flyInputTimer += Time.deltaTime;
            freezeFlying = true;
            if (flyInputTimer > spamDelay)
            {
                flyInputTimer = 0;
                flyInputPressed = false;
                freezeFlying = false;
            }
            else
                return;
        }

        if (Input.GetAxisRaw("Vertical") > 0 && canFly)
        {
            fireFX.Play();
            Fuel -= consumeSpeed * Time.deltaTime;
            delay = 0;
            Rigidbody.AddForce(Vector2.up * flyForce);
            flyInputPressed = true;

            if (Rigidbody.velocity.y > flyLimits.y)
            {
                Vector2 clamped = Rigidbody.velocity;
                clamped.y = flyLimits.y;
                Rigidbody.velocity = clamped;
            }
        }
        else if (MainState != PlayerState.InPropulsion)
            fireFX.Stop();
    }

    public bool OnGround()
    {
        return Physics2D.OverlapCircle(feetPoint.position, radius, groundMask);
    }

    public float GetFuelCapacity()
    {
        return Fuel / fuelCapacity;
    }

    private void Update()
    {
        ManageState();
        SlowReload();
        ManageInputs();

        if (MainState != PlayerState.IsSliding)
            animator.FlipSprite();

        if (Fuel <= 0 && !reloading)
            StartCoroutine(ReloadFuel());
    }

    public override void Death()
    {
        base.Death();
        VFXManager.Instance.PlayVFX("BloodVFX", transform.position);
    }

    private void FixedUpdate()
    {
        Movements();
        Flying();
        ClampVelocity();
    }
}