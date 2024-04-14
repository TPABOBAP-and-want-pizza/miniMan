using UnityEngine;

public class HandBehavior : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool hasCollided = false;
    private bool hasInteracted = false;  // Флаг интеракции с любым объектом, кроме "Ground"

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.CompareTag("Ground"))
        {
            Debug.Log(collider);
            // Тряска камеры и установка флага интеракции при столкновении с любым объектом, кроме "Ground"
            Camera.main.GetComponent<CameraShake>().DefaultShake();
            hasInteracted = true;
        }
        else if (collider.CompareTag("Ground") && hasInteracted)
        {
            // Зафиксировать позицию руки только после интеракции с другими объектами
            if (!hasCollided)
            {
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                hasCollided = true;
            }
        }
    }
}
