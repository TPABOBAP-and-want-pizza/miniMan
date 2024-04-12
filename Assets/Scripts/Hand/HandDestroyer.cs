using UnityEngine;

public class HandDestroyer : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 5f); // Уничтожить через 5 секунд
    }

}