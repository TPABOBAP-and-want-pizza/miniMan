using UnityEngine;

public class HandBehavior : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private GameObject player;
    private bool hasCollided = false;
    private bool hasInteracted = false;
    public bool TrackPlayer = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); // Убедитесь, что компонент Animator присутствует на этом же объекте

        // Изначально блокируем все движения и вращения руки
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (TrackPlayer)
        {
            player = GameObject.FindGameObjectWithTag("Player"); // Убедитесь, что ваш игрок имеет тег "Player"
        }
    }

    void Update()
    {
        if (TrackPlayer && player != null)
        {
            transform.position = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
        }
    }

    // Этот метод вызывается анимацией в определенный момент
    public void StartFalling()
    {
        // Убедитесь, что Rigidbody не является кинематическим, чтобы гравитация начала действовать
        rb.isKinematic = false;

        // Разблокируем все движения и вращения, позволяя руке начать падение
        rb.constraints = RigidbodyConstraints2D.None;
        Debug.Log("StartFalling вызван.");
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.CompareTag("Ground"))
        {
            Debug.Log(collider);
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
            }
        }
    }
}
