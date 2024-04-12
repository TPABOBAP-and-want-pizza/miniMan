using UnityEngine;

public class HandBehavior : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool hasCollided = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!hasCollided) // Проверяем, не столкнулась ли рука уже
        {
            // Зафиксировать позицию руки
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            hasCollided = true;

            // Вызов метода Shake из скрипта CameraShake на камере
            Camera.main.GetComponent<CameraShake>().DefaultShake();
        }
    }
}
