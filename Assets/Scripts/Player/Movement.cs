using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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
    [SerializeField] private float throwForce = 10f; // Сила броска

    public float jumpSpeed;

    //variables
    public bool grounded;
    float xInput;
    float yInput;
    public bool interactive_object_detected_in_front_of_character = false;
    private GameObject heldObject; // Ссылка на поднятый объект
    private bool isHoldingObject = false;

    public ParticleSystem dust;
    private float lastDirection = 1;

    //GOLF
    public Image powerIndicator; // Ссылка на UI индикатор
    private bool isCharging = false; // Проверка, идет ли зарядка
    private float chargeTime = 0f; // Текущее время зарядки
    private const float maxChargeTime = 1f; // Максимальное время зарядки
    private float throwForceMin = 5f; // Минимальная сила броска
    private float throwForceMax = 20f; // Максимальная сила броска
    private bool isThrowingObject = false;

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

        Camera aimingCamera = GameObject.FindGameObjectWithTag("Camera_Aiming").GetComponent<Camera>();
        if (aimingCamera == null)
        {
            Debug.LogError("Aiming camera not found!");
            return;
        }
        Vector2 mousePosition = aimingCamera.ScreenToWorldPoint(Input.mousePosition);

        Vector2 directionToMouse = (mousePosition - (Vector2)transform.position).normalized;
        powerIndicator.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg);

        if (Input.GetMouseButtonDown(0) && heldObject != null)
        {
            isCharging = true;
            chargeTime = 0;
        }
        if (isCharging)
        {
            chargeTime += Time.deltaTime;
            float pingPong = Mathf.PingPong(chargeTime, maxChargeTime);
            powerIndicator.fillAmount = pingPong / maxChargeTime;
        }
        if (Input.GetMouseButtonUp(0) && heldObject != null && isCharging)
        {
            ThrowHeldObject(Map(powerIndicator.fillAmount, 0, 1, throwForceMin, throwForceMax));
            isCharging = false;
            powerIndicator.fillAmount = 0;
        }

        bool isRunning = Mathf.Abs(xInput) > 0.1f; // Проверяем, двигается ли персонаж
        if (isHoldingObject && isRunning && grounded)
        {
            animator.Play("running_with_object_anim");
            animator.speed = 1; // Устанавливаем нормальную скорость анимации
        }
        else if (isHoldingObject && !isRunning)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("running_with_object_anim"))
            {
                animator.speed = 0; // Останавливаем анимацию на текущем кадре
            }
        }

        if (!isHoldingObject)
        {
            animator.speed = 1;
        }

        // Проверка бросания объекта
        if (isThrowingObject)
        {
            animator.Play("throwing_anim");
            StartCoroutine(StopThrowAnimation());
        }


        // (heldObject != null)
        //{
        //    UpdateHeldObjectPosition();
        //}
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
        if (bodyInteraction != null && (Input.GetKey(KeyCode.E) || interactive_object_detected_in_front_of_character) && grounded)
        {
            if (interactive_object_detected_in_front_of_character)
                animator.Play("Push");
            else animator.Play("Pull");

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
        {
            noise.PlayRandomNoise("Jump");
            dust.Play();
        }

        if (grounded)
        {
            if ((Input.GetKey(KeyCode.E) || interactive_object_detected_in_front_of_character) && CheckGroundIsInteractableObject())
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
                UpdateRunning(); // Обновленный метод для управления бегом
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
        if (!isHoldingObject)
        {
            animator.Play("Slink");
        }
 

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
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (heldObject != null)
            {
                DropHeldObject(); // Отпускаем объект
            }
            else
            {
                TryPickUpObject(); // Пытаемся поднять объект
            }
        }
    }


    void FaceInput()
    {
        float direction = Mathf.Sign(xInput);
        if (xInput != 0 && direction != lastDirection && grounded) // Добавили проверку, что персонаж на земле
        {
            dust.Play();  // Активация системы частиц при повороте
            lastDirection = direction;  // Обновляем последнее направление
        }
        transform.localScale = new Vector3(direction, 1, 1);
    }


    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && grounded)
        {
            body.velocity = new Vector2(body.velocity.x, jumpSpeed);
            dust.Play();
        }
    }

    void CheckGround()
    {
        grounded = Physics2D.OverlapAreaAll(groundCheck.bounds.min, groundCheck.bounds.max, groundMask).Length > 0;
    }

    void SetInteractableObject()
    {
        float _horizontalInput = xInput;

        RaycastHit2D hit = Physics2D.BoxCast(transform.position, boxCollider2DSize, 0f, Vector2.right, 0.05f, groundMask);

        if (hit.collider?.tag == "InteractableObject")
        {
            Rigidbody2D rb2 = hit.collider.GetComponent<Rigidbody2D>();
            interactionSpeed = -rb2.mass + maxXSpeed;
            bodyInteraction = rb2;
            return;
        }

        hit = Physics2D.BoxCast(transform.position, boxCollider2DSize, 0f, Vector2.left, 0.05f, groundMask);

        if (hit.collider?.tag == "InteractableObject")
        {
            Rigidbody2D rb2 = hit.collider.GetComponent<Rigidbody2D>();
            interactionSpeed = -rb2.mass + maxXSpeed;
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
        bool movingRight = xInput > 0;
        float checkDistance = 0.05f;
        Vector2 directionCheck = movingRight ? Vector2.right : Vector2.left;

        Vector2 boxSize = new Vector2(0.1f, boxCollider2DSize.y);
        Vector2 checkStartPoint = new Vector2(transform.position.x + (movingRight ? 0.2f : -0.2f), transform.position.y);

        RaycastHit2D hitInfo = Physics2D.BoxCast(checkStartPoint, boxSize, 0, directionCheck, checkDistance, groundMask);

        Debug.DrawRay(checkStartPoint, directionCheck * checkDistance, Color.red);

        if (hitInfo.collider != null && hitInfo.collider.CompareTag("InteractableObject"))
        {
            interactive_object_detected_in_front_of_character = true;
            //Debug.Log("TRUE");
        }
        else
        {
            interactive_object_detected_in_front_of_character = false;
            //Debug.Log("FALSE");
        }
    }

    bool CheckGroundIsInteractableObject()
    {
        Collider2D[] colliders = Physics2D.OverlapAreaAll(groundCheck.bounds.min, groundCheck.bounds.max, groundMask);
        foreach (Collider2D i in colliders)
        {
            if (i.tag == "InteractableObject")
                return false;
        }
        return true;
    }

    private void TryPickUpObject()
    {
        float checkDistance = 1f; // Расстояние проверки
        Vector2 boxSize = new Vector2(0.1f, 1f); // Размер box cast
        Vector2 direction = transform.right * Mathf.Sign(transform.localScale.x); // Направление в зависимости от ориентации персонажа
        Vector2 startPoint = (Vector2)transform.position + direction * 0.5f; // Стартовая точка впереди игрока

        RaycastHit2D hit = Physics2D.BoxCast(startPoint, boxSize, 0, direction, checkDistance);

        if (hit.collider != null && hit.collider.CompareTag("Throwing"))
        {
            heldObject = hit.collider.gameObject;
            Rigidbody2D rb = heldObject.GetComponent<Rigidbody2D>();
            rb.isKinematic = true; // Останавливаем все физические взаимодействия
            heldObject.transform.SetParent(transform);
            heldObject.transform.localPosition = Vector2.right * 0.5f; // Позиция у персонажа в руках
            Debug.Log("Object picked up: " + heldObject.name);

            Collider2D collider = heldObject.GetComponent<Collider2D>();
            collider.enabled = false;
            isHoldingObject = true;
        }
        else
        {
            Debug.Log("No throwable object in range to pick up.");
        }
    }

    private void ThrowHeldObject(float force)
    {
        if (heldObject != null)
        {
            isThrowingObject = true;
            Rigidbody2D rb = heldObject.GetComponent<Rigidbody2D>();
            rb.isKinematic = false;

            Camera aimingCamera = GameObject.FindGameObjectWithTag("Camera_Aiming").GetComponent<Camera>();
            if (aimingCamera == null)
            {
                Debug.LogError("Aiming camera not found!");
                return;
            }
            Vector2 mousePosition = aimingCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 throwDirection = (mousePosition - new Vector2(transform.position.x, transform.position.y)).normalized;
            rb.velocity = throwDirection * force;
            Debug.Log("Object thrown: " + heldObject.name + " at velocity: " + rb.velocity);

            Collider2D collider = heldObject.GetComponent<Collider2D>();
            collider.enabled = true;
            heldObject.transform.SetParent(null);
            heldObject = null;
            isHoldingObject = false;
        }
    }


    void DropHeldObject()
    {
        if (heldObject != null)
        {
            Collider2D collider = heldObject.GetComponent<Collider2D>();
            collider.enabled = true; // Включаем коллайдер обратно

            heldObject.transform.SetParent(null);
            Rigidbody2D rb = heldObject.GetComponent<Rigidbody2D>();
            rb.isKinematic = false;
            Debug.Log("Object dropped.");
            heldObject = null;

            isHoldingObject = false;
        }
    }

    private Vector3 GetNewPositionBasedOnInput()
    {
        // Предполагаем, что нужно использовать текущий ввод пользователя для определения позиции
        // Например, перемещать объект вместе с игроком
        return transform.position + new Vector3(xInput, 0, 0);
    }

    void UpdateHeldObjectPosition()
    {
        if (heldObject != null)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 directionToMouse = (mousePosition - (Vector2)transform.position).normalized;

            // Устанавливаем расстояние от игрока до объекта
            float heldDistance = 1.5f; // Расстояние от игрока до объекта
            Vector2 targetPosition = (Vector2)transform.position + directionToMouse * heldDistance;

            // Проверяем, нет ли препятствий между персонажем и новой позицией
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToMouse, heldDistance, groundMask);
            if (hit.collider != null)
            {
                // Если есть препятствие, остановить объект на границе препятствия до столкновения
                targetPosition = hit.point - directionToMouse * 0.1f; // небольшое смещение от препятствия
            }

            // Обновляем позицию объекта
            heldObject.transform.position = targetPosition;
        }
    }

    IEnumerator StopThrowAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        isThrowingObject = false;
        // Вернуться к другой релевантной анимации, в зависимости от состояния игрока
        if (Mathf.Abs(xInput) > 0.1f && grounded)
        {
            animator.Play("running_with_object_anim");
        }
        else
        {
            animator.Play("idle_anim");
        }
    }

    private void UpdateRunning()
    {
        if (isHoldingObject)
        {
            animator.Play("running_with_object_anim");
        }
        else
        {
            animator.Play("Run");
        }

        if (xInput == 0 || !grounded || Input.GetKey(KeyCode.LeftShift) || (Input.GetKey(KeyCode.E) || interactive_object_detected_in_front_of_character))
        {
            stateComplete = true;
        }
    }



}
