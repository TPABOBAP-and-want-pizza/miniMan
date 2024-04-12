using System;
using UnityEngine;

public class PressurePlate : MonoBehaviour, Transmitter
{
    public event Action OnActivate;
    public event Action OnDeactivate;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnActivate?.Invoke();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        OnDeactivate?.Invoke();
    }
}
