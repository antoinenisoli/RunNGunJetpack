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
}

public class PlayerController : MonoBehaviour
{
    [Header("Player")]
    public PlayerState MainState;
    [SerializeField] Health Health;
    [SerializeField] float speed;

    [Header("Fly")]
    [SerializeField] float flyForce;
    [SerializeField] float consumeSpeed = 3f, reloadSpeed = 1.5f;
    [SerializeField] float maxFallSpeed, maxFlyVelocity;

    [Header("Fuel")]
    [SerializeField] float reloadDelay;
    [SerializeField] float fuelCapacity = 50f;
    [SerializeField] Slider fuelSlider;

    [Header("Sliding")]
    [SerializeField] [Range(0,1)] float cancelThreshold = 0.3f;
    [SerializeField] [Curve(1,50)] AnimationCurve slidingCurve;
    [SerializeField] float slidingDuration = 3f;

    PlayerAnimator animator;
    float inputDirection;
    float delay;
    float fuel;
    Rigidbody2D rb;
    bool reloading;
    Dictionary<ModuleType, PlayerModule> modules = new Dictionary<ModuleType, PlayerModule>();

    public Rigidbody2D Rigidbody { get => rb; set => rb = value; }
    public float InputDirection { get => inputDirection; set => inputDirection = value; }
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

    private void Awake()
    {
        InputDirection = 1f;
        animator = GetComponentInChildren<PlayerAnimator>();
        Rigidbody = GetComponent<Rigidbody2D>();
        Fuel = fuelCapacity;

        foreach (var item in GetComponents<Propulsion>())
        {
            item.Initialize(this);
            modules.Add(item.myType, item);
        }
    }

    #region
    public bool TryGetModule(ModuleType moduleType, out PlayerModule module)
    {
        if (modules.TryGetValue(moduleType, out module))
            return true;

        return false;
    }

    public void UseModule(ModuleType moduleType)
    {
        if (TryGetModule(moduleType, out PlayerModule module))
            module.Use();
    }
    #endregion

    public void SetState(PlayerState newState)
    {
        MainState = newState;
        animator.SetAnimState(newState);
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

        Vector2 move = new Vector2();
        move.x = Input.GetAxisRaw("Horizontal") * speed;
        move.y = Rigidbody.velocity.y;
        Rigidbody.velocity = move;
        if (Input.GetAxisRaw("Horizontal") != 0)
            InputDirection = Input.GetAxisRaw("Horizontal");

        Vector2 clampedVelocity = Rigidbody.velocity;
        if (Fuel > 0 && !reloading && Input.GetKey(KeyCode.UpArrow))
        {
            Rigidbody.AddForce(Vector2.up * flyForce, ForceMode2D.Force);
            Fuel -= consumeSpeed * Time.deltaTime;
            delay = 0;
            clampedVelocity.y = Mathf.Clamp(clampedVelocity.y, maxFallSpeed, maxFlyVelocity);
        }
        else
            clampedVelocity.y = Mathf.Clamp(clampedVelocity.y, maxFallSpeed, Mathf.Infinity);

        Rigidbody.velocity = clampedVelocity;
    }

    public void ResetVelocity()
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
        if (MainState == PlayerState.IsSliding)
            return;

        if (Rigidbody.velocity.x != 0)
            SetState(PlayerState.IsRunning);
        else
            SetState(PlayerState.Idle);

        if (Rigidbody.velocity.y < 0)
            SetState(PlayerState.IsFalling);
        else if (Rigidbody.velocity.y > 0)
            SetState(PlayerState.IsFlying);
    }

    IEnumerator Slide()
    {
        ResetVelocity();
        SetState(PlayerState.IsSliding);
        float timer = 0;

        while (timer < slidingDuration)
        {
            yield return null;
            timer += Time.deltaTime;
            float step = timer / slidingDuration;
            float force = slidingCurve.Evaluate(step);

            Vector2 newVelocity = Rigidbody.velocity;
            newVelocity.x = InputDirection * force;
            Rigidbody.velocity = newVelocity;

            //print(step);
            if (step >= cancelThreshold && Input.GetKeyDown(KeyCode.LeftShift))
            {
                print("super propulsion");
                UseModule(ModuleType.Propulsion);
                break;
            }
        }

        SetState(PlayerState.Idle);
    }

    void ManageInputs()
    {
        if (MainState == PlayerState.IsSliding)
            return;

        if (Input.GetKeyDown(KeyCode.LeftShift))
            UseModule(ModuleType.Propulsion);

        if (Input.GetKeyDown(KeyCode.Space))
            StartCoroutine(Slide());
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
}
