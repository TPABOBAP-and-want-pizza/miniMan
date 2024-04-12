using System;
using UnityEngine;

public class Movement : MonoBehaviour
{
    enum PlayerState { Idle, Running, Airborne }
    PlayerState state;
    bool stateComplete;

    [SerializeField] Animator animator;

    //scene instanced objects
    public Rigidbody2D body;
    public BoxCollider2D groundCheck;
    public LayerMask groundMask;

    //movement properties
    public float acceleration;
    [Range(0f, 1f)]
    public float groundDecay;
    public float maxXSpeed;

    public float jumpSpeed;

    //variables
    public bool grounded;
    float xInput;
    float yInput;

    void Update()
    {
        CheckInput();
        HandleJump();

        if (stateComplete)
        {
            SelectState();
        }
        UpdateState();
    }

    void FixedUpdate()
    {
        CheckGround();
        HandleXMovement();
        ApplyFriction();
    }

    private void SelectState()
    {
        stateComplete = false;
        if (grounded)
        {
            if (xInput == 0)
            {
                state = PlayerState.Idle;
            }
            else
            {
                state = PlayerState.Running;
            }
        }
        else state = PlayerState.Airborne;
    }

    private void UpdateState()
    {
        switch (state)
        {
            case PlayerState.Idle:
                UpdateIdle();
                break;
            case PlayerState.Running:
                UpdateRun();
                break;
            case PlayerState.Airborne:
                UpdateAirborne();
                break;

        }
    }

    private void UpdateIdle()
    {
        //animator.Play("Idle");
        if (xInput != 0 || !grounded)
        {
            stateComplete = true;
        }
    }

    private void UpdateRun()
    {
        float velx = body.velocity.x;
        animator.speed = MathF.Abs(velx) / maxXSpeed;

        //animator.Play("Run");
        if (MathF.Abs(velx) < 0.1f || !grounded)
        {
            stateComplete = true;
        }
    }

    private void UpdateAirborne()
    {
        float time = Map(body.velocity.y, jumpSpeed, -jumpSpeed, 0, 1, true);
        //animator.Play("Jump", 0, time);
        animator.speed = 0;

        if (grounded)
        {
            stateComplete = true;
        }
    }

    void CheckInput()
    {
        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");
    }

    void HandleXMovement()
    {
        if (Mathf.Abs(xInput) > 0)
        {

            //increment velocity by our accelleration, then clamp within max
            float increment = xInput * acceleration;
            float newSpeed = Mathf.Clamp(body.velocity.x + increment, -maxXSpeed, maxXSpeed);
            body.velocity = new Vector2(newSpeed, body.velocity.y);

            FaceInput();
        }
    }

    void FaceInput()
    {
        float direction = Mathf.Sign(xInput);
        transform.localScale = new Vector3(direction, 1, 1);
    }

    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && grounded)
        {
            body.velocity = new Vector2(body.velocity.x, jumpSpeed);
        }
    }

    void CheckGround()
    {
        grounded = Physics2D.OverlapAreaAll(groundCheck.bounds.min, groundCheck.bounds.max, groundMask).Length > 0;
    }

    void ApplyFriction()
    {
        if (grounded && xInput == 0)
        {
            body.velocity = new Vector2(body.velocity.x * groundDecay, body.velocity.y);
        }
    }

    public static float Map(float value, float min1, float max1, float min2, float max2, bool clamp = false)
    {
        float val = min2 + (max2 - min2) * ((value - min1) / (max1 - min1));
        return clamp ? Mathf.Clamp(val, Mathf.Min(min2, max2), Mathf.Max(min2, max2)) : val;
    }
}
