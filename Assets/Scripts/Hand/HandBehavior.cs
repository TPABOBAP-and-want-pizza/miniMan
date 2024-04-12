using UnityEngine;

public class HandBehavior : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool hasCollided = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            // Тряска камеры, но рука продолжает движение
            Camera.main.GetComponent<CameraShake>().DefaultShake();
        }
        else if (!hasCollided) // Столкновение с любым другим объектом, кроме игрока
        {
            // Зафиксировать позицию руки
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            hasCollided = true;
        }
    }
}
