using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [SerializeField] Transform objectTransform;
    [SerializeField] string objectTag;
    [SerializeField] float movingSpeed = 1f; // ������ �������� ��� �������� �������
    [SerializeField] bool arena = false;
    private Vector3 arenaPosition;
    private float yOffset = 2f;

    public bool Arena
    {
        set { arena = value; }
    }

    void Start()
    {
        if (objectTransform == null)
        {
            if (objectTag == "")
            {
                objectTag = "Player";
            }

            GameObject playerObject = GameObject.FindGameObjectWithTag(objectTag);
            if (playerObject != null)
            {
                objectTransform = playerObject.transform;
            }
            else
            {
                Debug.LogError("Player object not found with tag: " + objectTag);
            }
        }
    }

    void Update()
    {
        if (objectTransform && !arena)
        {
            Approach(objectTransform.position);
        }
        else
        {
            yOffset = 0f;
            if (transform.position != arenaPosition)
            {
                Approach(arenaPosition);
            }
        }
    }

    private void Approach(Vector3 targetPosition)
    {
        Vector3 target = new Vector3(targetPosition.x, targetPosition.y + yOffset, -10f);
        Vector3 newPosition = Vector3.Lerp(transform.position, target, movingSpeed * Time.deltaTime);
        transform.position = newPosition;
    }
}
