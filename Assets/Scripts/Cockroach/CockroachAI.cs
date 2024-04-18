using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CockroachAI : MonoBehaviour
{
    public float speed = 5f; // Скорость движения врага
    public float checkDistance = 1f; // Дистанция проверки препятствия перед врагом
    private bool isMovingRight = true; // Направление движения врага

    private Rigidbody2D rb;
    public LayerMask groundLayer; // Слой, на котором находится земля

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Move();
        CheckForObstacle();
    }

    void Move()
    {
        float moveDirection = isMovingRight ? 1f : -1f; // Определение направления движения
        rb.velocity = new Vector2(moveDirection * speed, rb.velocity.y);
    }

    void CheckForObstacle()
    {
        // Направление проверки
        Vector2 direction = isMovingRight ? Vector2.right : Vector2.left;

        // Позиция для начала рейкаста
        Vector2 startPosition = new Vector2(transform.position.x, transform.position.y);

        RaycastHit2D hit = Physics2D.Raycast(startPosition, direction, checkDistance, groundLayer);
        Debug.DrawRay(startPosition, direction * checkDistance, Color.red); // Отображение рейкаста в редакторе

        if (hit.collider != null)
        {
            // Разворот на 180 градусов при обнаружении препятствия
            isMovingRight = !isMovingRight;
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y); // Поворот визуальной модели врага
        }
    }
}