using System;
using UnityEngine;

public class Movement : MonoBehaviour
{

    [SerializeField] RandomNoise noise;
    enum PlayerState { Idle, Running, Slink, Airborne, Interaction }
    PlayerState state;
    bool stateComplete = true;

    [SerializeField] Animator animator;
    private Rigidbody2D bodyInteraction;

    //scene instanced objects
    public Rigidbody2D body;
    public BoxCollider2D groundCheck;
    public LayerMask groundMask;
    private Vector2 boxCollider2DSize;

    //movement properties
    public float maxXSpeed;
    [SerializeField] float airSpeed;
    [SerializeField] float retardingSlink = 2f;
    [SerializeField] float interactionSpeed;

    public float jumpSpeed;

    //variables
    public bool grounded;
    float xInput;
    float yInput;
    bool interactive_object_detected_in_front_of_character = false;

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
        CheckForObjectInFront();
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

        RaycastHit2D hit = Physics2D.BoxCast(transform.position, boxCollider2DSize, 0f, Vector2.right * Mathf.Sign(_horizontalInput), 0.05f, groundMask);
        //Debug.Log($"hit collider - {hit.collider.gameObject}");

        if (hit.collider != null && bodyInteraction == null)
        {
            _horizontalInput = 0f;
        }
        if (bodyInteraction != null && (Input.GetKey(KeyCode.E)|| interactive_object_detected_in_front_of_character) && grounded)
        {
            body.velocity = new Vector2(_horizontalInput * interactionSpeed * 70f * Time.deltaTime, body.velocity.y);
            bodyInteraction.velocity = new Vector2(body.velocity.x, bodyInteraction.velocity.y);
        }
        else if (MathF.Abs(body.velocity.x) < maxXSpeed && grounded && Input.GetKey(KeyCode.LeftShift))
        {
            body.velocity = new Vector2(_horizontalInput * maxXSpeed * 70f * Time.deltaTime / retardingSlink, body.velocity.y);
        }
        else if (MathF.Abs(body.velocity.x) < maxXSpeed && grounded)
        {
            body.velocity = new Vector2(_horizontalInput * maxXSpeed * 50f * Time.deltaTime, body.velocity.y);
        }
        else if (_horizontalInput > 0 && body.velocity.x < airSpeed)
        {
            body.velocity += new Vector2(_horizontalInput * airSpeed * 10f * Time.deltaTime, 0);
        }
        else if (_horizontalInput < 0 && body.velocity.x > -airSpeed)
        {
            body.velocity += new Vector2(_horizontalInput * airSpeed * 10f * Time.deltaTime, 0);
        }
        FaceInput();
    }

    private void SelectState()
    {
        stateComplete = false;
        if (state == PlayerState.Airborne)
            noise.PlayRandomNoise("Jump");

        if (grounded)
        {
            if (Input.GetKey(KeyCode.E) || interactive_object_detected_in_front_of_character)
            {
                SetInteractableObject();
                if (bodyInteraction != null)
                    state = PlayerState.Interaction;
            }
            else if (xInput == 0)
            {
                state = PlayerState.Idle;
                animator.Play("Idle");
            }
            else if (Input.GetKey(KeyCode.LeftShift))
            {
                state = PlayerState.Slink;
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
            case PlayerState.Slink:
                UpdateSlink();
                break;
            case PlayerState.Airborne:
                UpdateAirborne();
                break;
            case PlayerState.Interaction:
                UpdateInteraction();
                break;

        }
    }

    private void UpdateIdle()
    {
        if (xInput != 0 || !grounded)
        {
            stateComplete = true;
        }
    }

    private void UpdateRun()
    {
        animator.Play("Run");
        if (xInput == 0 || !grounded || Input.GetKey(KeyCode.LeftShift) || (Input.GetKey(KeyCode.E) || interactive_object_detected_in_front_of_character))
        {
            stateComplete = true;
        }
    }

    private void UpdateSlink()
    {
        animator.Play("Slink");
        if (xInput == 0 || !grounded || Input.GetKeyUp(KeyCode.LeftShift) || (Input.GetKey(KeyCode.E) || interactive_object_detected_in_front_of_character))
        {
            stateComplete = true;
        }
    }

    private void UpdateAirborne()
    {
        if (body.velocity.y > 0)
        {
            animator.Play("Jump");
        }
        else animator.Play("Fall");

        if (grounded)
        {
            stateComplete = true;
        }
    }

    private void UpdateInteraction()
    {
        if ((!Input.GetKey(KeyCode.E) && !interactive_object_detected_in_front_of_character) || !grounded)
        {
            bodyInteraction = null;
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

    void SetInteractableObject()
    {
        Debug.Log("22");
        float _horizontalInput = xInput;

        RaycastHit2D hit = Physics2D.BoxCast(transform.position, boxCollider2DSize, 0f, Vector2.right, 0.05f, groundMask);

        if (hit.collider?.tag == "InteractableObject")
        {
            Rigidbody2D rb2 = hit.collider.GetComponent<Rigidbody2D>();
            interactionSpeed = -body.mass * rb2.mass / 100f + maxXSpeed;
            bodyInteraction = rb2;
            return;
        }

        hit = Physics2D.BoxCast(transform.position, boxCollider2DSize, 0f, Vector2.left, 0.05f, groundMask);

        if (hit.collider?.tag == "InteractableObject")
        {
            Rigidbody2D rb2 = hit.collider.GetComponent<Rigidbody2D>();
            interactionSpeed = -body.mass * rb2.mass / 100f + maxXSpeed;
            bodyInteraction = rb2;
        }
    }

    public static float Map(float value, float min1, float max1, float min2, float max2, bool clamp = false)
    {
        float val = min2 + (max2 - min2) * ((value - min1) / (max1 - min1));
        return clamp ? Mathf.Clamp(val, Mathf.Min(min2, max2), Mathf.Max(min2, max2)) : val;
    }

    void CheckForObjectInFront()
    {
        bool movingRight = xInput > 0;  // Определение направления движения персонажа
        float checkDistance = 0.1f;     // Расстояние проверки перед персонажем
        Vector2 directionCheck = movingRight ? Vector2.right : Vector2.left;  // Направление проверки

        // Изменяем размеры BoxCast для лучшего обнаружения объектов разных размеров
        Vector2 boxSize = new Vector2(0.2f, boxCollider2DSize.y); // Уменьшенная ширина и использование высоты персонажа
        Vector2 checkStartPoint = new Vector2(transform.position.x + (movingRight ? 0.5f : -0.5f), transform.position.y); // Стартовая точка BoxCast чуть впереди персонажа

        // Выполнение BoxCast
        RaycastHit2D hitInfo = Physics2D.BoxCast(checkStartPoint, boxSize, 0, directionCheck, checkDistance, groundMask);

        // Визуализация BoxCast в редакторе Unity
        Debug.DrawRay(checkStartPoint, directionCheck * checkDistance, Color.red);

        if (hitInfo.collider != null && hitInfo.collider.CompareTag("InteractableObject"))
        {
            interactive_object_detected_in_front_of_character = true;
            Debug.Log("TRUE");
        }
        else
        {
            interactive_object_detected_in_front_of_character = false;
            Debug.Log("FALSE");
        }
    }


}
