using System;
using UnityEngine;

public class PressurePlate : MonoBehaviour, Transmitter
{
    public event Action OnSetActive;
    public event Action OnSetUnactive;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnSetActive?.Invoke();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        OnSetUnactive?.Invoke();
    }
}
