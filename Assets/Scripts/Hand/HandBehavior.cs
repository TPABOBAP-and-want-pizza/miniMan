using UnityEngine;

public class HandBehavior : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private GameObject player;
    private bool hasCollided = false;
    private bool hasInteracted = false;
    public bool TrackPlayer = false;
    private bool is_start_falling = false;

    public float shockwaveRadius = 5f; // Радиус действия волны
    public float shockwaveForce = 10f; // Сила, с которой объекты будут отталкиваться

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        if (TrackPlayer)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
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

    public void StartFalling()
    {
        rb.isKinematic = false;
        is_start_falling = true;
        rb.constraints = RigidbodyConstraints2D.None;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.CompareTag("Ground"))
        {
            Camera.main.GetComponent<CameraShake>().DefaultShake();
            hasInteracted = true;
        }
        else if (collider.CompareTag("Ground") && hasInteracted)
        {
            if (!hasCollided)
            {
                TrackPlayer = false;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                hasCollided = true;
                CauseShockwave();
            }
        }
    }

    void CauseShockwave()
    {
        Vector2 explosionPos = transform.position;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(explosionPos, shockwaveRadius);
        foreach (Collider2D hit in colliders)
        {
            Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
            if (rb != null && rb != this.rb) // Убедитесь, что это не Rigidbody самой руки
            {
                Vector2 direction = hit.transform.position - transform.position;
                rb.AddForce(direction.normalized * shockwaveForce + Vector2.up * shockwaveForce, ForceMode2D.Impulse);
            }
        }
    }
}
