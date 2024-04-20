using UnityEngine;

public class HandDestroyer : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 2f);
    }

}