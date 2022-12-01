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
    [SerializeField] float flyForce, propulsionForce = 15f;
    [SerializeField] float consumeSpeed = 3f, reloadSpeed = 1.5f;
    [SerializeField] float maxFallSpeed, maxFlyVelocity;

    [Header("Fuel")]
    [SerializeField] float reloadDelay;
    [SerializeField] float fuelCapacity = 50f;
    [SerializeField] Slider fuelSlider;

    [Header("Sliding")]
    [SerializeField] float slidingSpeed = 5f;
    [SerializeField] float slidingDuration = 3f;

    PlayerAnimator animator;
    float inputDirection;
    float delay;
    float fuel;
    Rigidbody2D rb;
    bool reloading;

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
        animator = GetComponentInChildren<PlayerAnimator>();
        rb = GetComponent<Rigidbody2D>();
        Fuel = fuelCapacity;
    }

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
        move.y = rb.velocity.y;
        rb.velocity = move;
        if (MainState != PlayerState.IsSliding)
            inputDirection = Input.GetAxisRaw("Horizontal");
        else
            inputDirection = 1f;

        Vector2 clampedVelocity = rb.velocity;
        if (Fuel > 0 && !reloading && Input.GetKey(KeyCode.UpArrow))
        {
            rb.AddForce(Vector2.up * flyForce, ForceMode2D.Force);
            Fuel -= consumeSpeed * Time.deltaTime;
            delay = 0;
            clampedVelocity.y = Mathf.Clamp(clampedVelocity.y, maxFallSpeed, maxFlyVelocity);
        }
        else
            clampedVelocity.y = Mathf.Clamp(clampedVelocity.y, maxFallSpeed, Mathf.Infinity);

        rb.velocity = clampedVelocity;
    }

    void ResetVelocity()
    {
        Vector2 newVelocity = rb.velocity;
        newVelocity.y = 0;

        rb.velocity = newVelocity;
    }

    void Propulsion()
    {
        ResetVelocity();
        rb.AddForce(Vector2.up * propulsionForce, ForceMode2D.Impulse);
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

        if (rb.velocity.x != 0)
            SetState(PlayerState.IsRunning);
        else
            SetState(PlayerState.Idle);

        if (rb.velocity.y < 0)
            SetState(PlayerState.IsFalling);
        else if (rb.velocity.y > 0)
            SetState(PlayerState.IsFlying);
    }

    IEnumerator Slide()
    {
        SetState(PlayerState.IsSliding);
        float timer = 0;

        while (timer < slidingDuration)
        {
            timer += Time.deltaTime;
            yield return null;
            rb.AddForce((Vector2.right * inputDirection) * slidingSpeed);
        }

        SetState(PlayerState.Idle);
    }

    void ManageInputs()
    {
        if (MainState == PlayerState.IsSliding)
            return;

        if (Input.GetKeyDown(KeyCode.LeftShift))
            Propulsion();

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
