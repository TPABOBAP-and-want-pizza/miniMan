using System;
using UnityEngine;

public class PressurePlate : MonoBehaviour, Transmitter
{
    public event Action OnActivate;
    public event Action OnDeactivate;
    public Animator animator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        animator.Play("ActivatePlane");
        OnActivate?.Invoke();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        animator.Play("DeactivatePlane");
        OnDeactivate?.Invoke();
    }
}
