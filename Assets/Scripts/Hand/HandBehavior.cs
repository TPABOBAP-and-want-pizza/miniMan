using UnityEngine;

public class HandBehavior : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private GameObject player;
    private Collider2D handCollider;  // Добавляем ссылку на Collider
    private GameObject firstInteractor;
    private bool hasCollided = false;
    private bool hasInteracted = false;
    public bool TrackPlayer = false;
    private bool is_start_falling = false;

    public float shockwaveRadius = 5f;
    public float shockwaveForce = 10f;
    public ParticleSystem dust_hand;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        handCollider = GetComponent<Collider2D>();  // Получаем Collider
        handCollider.enabled = false;  // Отключаем Collider в начале
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        if (TrackPlayer)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    // Метод для включения Collider
    public void EnableCollider()
    {
        handCollider.enabled = true;
    }

    public void StartFalling()
    {
        rb.isKinematic = false;
        is_start_falling = true;
        rb.constraints = RigidbodyConstraints2D.None;
        EnableCollider();  // Активируем коллайдер сразу при падении
    }

    void Update()
    {
        if (TrackPlayer && player != null)
        {
            if (is_start_falling)
            {
                transform.position = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
            }
            else
            {
                transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 2, transform.position.z);
            }
        }
    }


    void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.CompareTag("Ground"))
        {
            if (!hasInteracted) // Сохраняем первый объект, который столкнулся с рукой
            {
                firstInteractor = collider.gameObject;
            }
            hasInteracted = true;
            //Camera.main.GetComponent<CameraShake>().DefaultShake();
        }
        else if (collider.CompareTag("Ground") && hasInteracted)
        {
            if (!hasCollided)
            {
                TrackPlayer = false;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                hasCollided = true;
                CauseShockwave();
                dust_hand.Play();
                Camera.main.GetComponent<CameraShake>().DefaultShake();
            }
        }
    }

    void CauseShockwave()
    {
        Vector2 explosionPos = transform.position;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(explosionPos, shockwaveRadius);
        foreach (Collider2D hit in colliders)
        {
            if (hit.gameObject == firstInteractor) // Игнорируем первый объект, который столкнулся с рукой
            {
                continue;
            }

            Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
            if (rb != null && rb != this.rb) // Убедитесь, что это не Rigidbody самой руки
            {
                Vector2 direction = (hit.transform.position - transform.position).normalized;
                Vector2 force = new Vector2(direction.x * shockwaveForce, (direction.y + 2) * shockwaveForce);
                rb.AddForce(force, ForceMode2D.Impulse);
            }
        }
    }
}
