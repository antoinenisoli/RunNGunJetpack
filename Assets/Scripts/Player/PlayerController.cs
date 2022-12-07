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
    [SerializeField] float propulsionDelay = 0.5f;
    [SerializeField] float flyForce;
    [SerializeField] float consumeSpeed = 3f, reloadSpeed = 1.5f;

    [Header("Fuel")]
    [SerializeField] float reloadDelay;
    [SerializeField] float fuelCapacity = 50f;
    [SerializeField] Slider fuelSlider;

    PlayerAnimator animator;
    float inputDirection;
    float delay;
    float fuel;
    Rigidbody2D rb;
    bool reloading;
    Dictionary<ModuleType, PlayerModule> modules = new Dictionary<ModuleType, PlayerModule>();
    Vector2 move;

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

    public bool UseModule(ModuleType moduleType)
    {
        bool getModule = TryGetModule(moduleType, out PlayerModule module);
        if (getModule)
            module.Use();
        
        return getModule;
    }

    public void Propulsion()
    {
        if (UseModule(ModuleType.Propulsion))
        {
            SetState(PlayerState.InPropulsion);
            StartCoroutine(SetStateDelayed(PlayerState.Idle, propulsionDelay));
            fireFX.Play();
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
        }
    }

    void ClampVelocity()
    {
        move = Rigidbody.velocity;
        move.y = Mathf.Clamp(move.y, flyLimits.x, Mathf.Infinity);
        if (MainState != PlayerState.IsSliding && Input.GetAxisRaw("Horizontal") == 0) //deceleration
        {
            if (OnGround())
                move.x = Mathf.Lerp(move.x, 0, groundDeceleration * Time.fixedDeltaTime);
            else
                move.x = Mathf.Lerp(move.x, 0, airDeceleration * Time.fixedDeltaTime);
        }

        if (MainState != PlayerState.IsSliding)
            move.x = Mathf.Clamp(move.x, walkLimits.x, walkLimits.y);

        Rigidbody.velocity = move;
    }

    public void ResetYVelocity()
    {
        Vector2 newVelocity = Rigidbody.velocity;
        newVelocity.y = 0;

        Rigidbody.velocity = newVelocity;
    }

    void SlowReload()
    {
        delay += Time.deltaTime;
        if (delay > reloadDelay && !reloading)
            Fuel += reloadSpeed * Time.deltaTime;
    }

    void ManageState()
    {
        if (MainState == PlayerState.IsSliding || MainState == PlayerState.InPropulsion)
            return;

        float walkThreshold = 0.2f;
        if (Rigidbody.velocity.x > walkThreshold || Rigidbody.velocity.x < -walkThreshold)
            SetState(PlayerState.IsRunning);
        else
            SetState(PlayerState.Idle);

        if (Rigidbody.velocity.y < 0)
            SetState(PlayerState.IsFalling);
        else if (Rigidbody.velocity.y > 0)
            SetState(PlayerState.IsFlying);
    }

    void ManageInputs()
    {
        if (MainState == PlayerState.IsSliding)
            return;

        if (Input.GetKeyDown(KeyCode.LeftShift))
            Propulsion();

        if (Input.GetKeyDown(KeyCode.Space))
            UseModule(ModuleType.Slide);
    }

    void Flying()
    {
        bool correctState = MainState != PlayerState.IsSliding && MainState != PlayerState.InPropulsion;
        bool canFly = correctState && Fuel > 0 && !reloading;

        if (Input.GetAxisRaw("Vertical") > 0 && canFly)
        {
            fireFX.Play();
            Fuel -= consumeSpeed * Time.deltaTime;
            delay = 0;
            Rigidbody.AddForce(Vector2.up * flyForce);
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

    private void Update()
    {
        ManageState();
        Movements();
        SlowReload();
        ManageInputs();
        if (MainState != PlayerState.IsSliding)
            animator.FlipSprite();

        if (Fuel <= 0 && !reloading)
            StartCoroutine(ReloadFuel());

        if (fuelSlider)
        {
            float value = Fuel / fuelCapacity;
            fuelSlider.value = value;
        }
    }

    private void FixedUpdate()
    {
        Flying();
        ClampVelocity();
    }
}
