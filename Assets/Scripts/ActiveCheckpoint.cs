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
            // ������������ ��������� �������
            DeactivatePreviousCheckpoint();

            // ������ �������� ������� �� ��������
            isActive = true;
            UpdateSprite();
        }
    }

    private void DeactivatePreviousCheckpoint()
    {
        // ������ �� �������� �� ����
        ActiveCheckpoint[] checkpoints = FindObjectsOfType<ActiveCheckpoint>();

        // �������� �� ������� �������� � ������������ ����, ���� �� ��� ��������
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
        // ���������� ������ �������� �� ������� ��������
        spriteRenderer.sprite = isActive ? activeSprite : inactiveSprite;
    }
}
