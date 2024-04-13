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
    private Vector2 boxCollider2DSize;

    //movement properties
    public float maxXSpeed;
    [SerializeField] float airSpeed;

    public float jumpSpeed;

    //variables
    public bool grounded;
    float xInput;
    float yInput;

    private void Start()
    {
        boxCollider2DSize = transform.GetComponent<BoxCollider2D>().size;
        boxCollider2DSize = new Vector2(boxCollider2DSize.x - 0.02f, boxCollider2DSize.y - 0.02f);
    }

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
        Move();
    }

    private void Move()
    {
        if (xInput == 0)
            return;

        float _horizontalInput = xInput;

        RaycastHit2D hit = Physics2D.BoxCast(transform.position, boxCollider2DSize, 0f, Vector2.right * Mathf.Sign(_horizontalInput), 0.02f, groundMask);
        //Debug.Log($"hit collider - {hit.collider.gameObject}");

        if (hit.collider != null)
        {
            _horizontalInput = 0f;
        }
        if (MathF.Abs(body.velocity.x) < maxXSpeed && grounded)
        {
            body.velocity = new Vector2(_horizontalInput * maxXSpeed * 100 * Time.deltaTime, body.velocity.y);
        }
        else if (_horizontalInput > 0 && body.velocity.x < maxXSpeed)
        {
            body.velocity += new Vector2(_horizontalInput * airSpeed * 10 * Time.deltaTime, 0);
        }
        else if (_horizontalInput < 0 && body.velocity.x > -maxXSpeed)
        {
            body.velocity += new Vector2(_horizontalInput * airSpeed * 10 * Time.deltaTime, 0);
        }
        FaceInput();
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

    public static float Map(float value, float min1, float max1, float min2, float max2, bool clamp = false)
    {
        float val = min2 + (max2 - min2) * ((value - min1) / (max1 - min1));
        return clamp ? Mathf.Clamp(val, Mathf.Min(min2, max2), Mathf.Max(min2, max2)) : val;
    }
}
