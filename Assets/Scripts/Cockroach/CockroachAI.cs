using UnityEngine;

public class CockroachAI : MonoBehaviour
{
    public LayerMask groundLayer; //стены
    public Transform groundDetection; //точка для обнаружения земли впереди
    private Rigidbody2D rb;
    private Animator animator;
    public float speed = 5.0f;
    public float checkDistance = 1.4f;
    private bool movingRight = true;
    public bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        // Проверка, заканчивается ли земля перед врагом
        Vector2 groundDirectionCheck = movingRight ? Vector2.right : Vector2.left;
        Vector2 groundCheckStart = new Vector2(transform.position.x + (movingRight ? 1.0f : -1.0f), transform.position.y);
        RaycastHit2D groundInfo = Physics2D.Raycast(groundCheckStart, Vector2.down, checkDistance, groundLayer);
        Debug.DrawRay(groundCheckStart, Vector2.down * checkDistance, Color.green);

        if (!groundInfo.collider)
        {
            Flip();
        }

        // Проверка на столкновение со стеной перед врагом
        RaycastHit2D wallInfo = Physics2D.Raycast(transform.position, groundDirectionCheck, checkDistance, groundLayer);
        Debug.DrawRay(transform.position, groundDirectionCheck * checkDistance, Color.red);

        if (wallInfo.collider)
        {
            Flip();
        }
    }

    // Поворот врага
    private void Flip()
    {
        movingRight = !movingRight;
        transform.eulerAngles = new Vector3(0, movingRight ? 0 : 180, 0);
    }

    public void Die()
    {
        isDead = true;
        animator.SetTrigger("Die"); // Установка триггера смерти в аниматоре
        rb.velocity = Vector2.zero; // Остановить движение
        rb.isKinematic = false; // Включаем физику (если требуется)
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("DeadlyObject"))
        {
            float impactForce = collision.relativeVelocity.magnitude * collision.rigidbody.mass;

            if (impactForce > 10) // Предполагаемое значение, необходимое для смерти
            {
                Die();
            }
        }
    }

}
