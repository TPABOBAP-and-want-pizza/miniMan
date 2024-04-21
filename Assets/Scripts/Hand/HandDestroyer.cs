using UnityEngine;

public class HandDestroyer : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 2f);
    }

    public void DestroyHand()
    {
        Destroy(gameObject);
    }

}