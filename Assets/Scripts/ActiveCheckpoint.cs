using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveCheckpoint : MonoBehaviour
{
    public Sprite activeSprite;
    public Sprite inactiveSprite;

    private SpriteRenderer spriteRenderer;
    private bool isActive = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateSprite();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Деактивувати попередній чекпоінт
            DeactivatePreviousCheckpoint();

            // Змінити поточний чекпоінт на активний
            isActive = true;
            UpdateSprite();
        }
    }

    private void DeactivatePreviousCheckpoint()
    {
        // Знайти всі чекпоінти на сцені
        ActiveCheckpoint[] checkpoints = FindObjectsOfType<ActiveCheckpoint>();

        // Пройтися по кожному чекпоінту і деактивувати його, якщо він був активний
        foreach (ActiveCheckpoint checkpoint in checkpoints)
        {
            if (checkpoint != this && checkpoint.isActive)
            {
                checkpoint.isActive = false;
                checkpoint.UpdateSprite();
            }
        }
    }

    private void UpdateSprite()
    {
        // Встановити спрайт відповідно до статусу чекпоінту
        spriteRenderer.sprite = isActive ? activeSprite : inactiveSprite;
    }
}
