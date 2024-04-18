using System;
using UnityEngine;

public class PressurePlate : MonoBehaviour, Transmitter
{
    public event Action OnActivate;
    public event Action OnDeactivate;
    public Animator animator;
    private int countEnter = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        countEnter++;
        if(countEnter == 1)
            animator.Play("ActivatePlane");
        if(countEnter == 1)
            OnActivate?.Invoke();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        countEnter--;
        if(countEnter == 0)
            animator.Play("DeactivatePlane");
        if(countEnter == 0)
            OnDeactivate?.Invoke();
    }
}
