using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [SerializeField] Transform objectTransform;
    [SerializeField] string objectTag;
    [SerializeField] float movingSpeed = 0.1f;
    [SerializeField] bool arena = false;
    private Vector3 arena_position;
    private float y_offset = 2;
    public bool Arena { set { arena = value; } }
    void Start()
    {
        if (objectTransform == null)
        {
            if (objectTag == "")
            {
                objectTag = "Player";
            }

            objectTransform = GameObject.FindGameObjectWithTag(objectTag).transform;
        }
        arena_position = new Vector3(49, -48, -10);
    }
    void Update()
    {
        if (objectTransform && !arena)
        {
            Approach(objectTransform.position);
        }
        else
        {
            y_offset = 0;
            if (transform.position != arena_position)
            {
                Approach(arena_position);
            }
            if (gameObject.GetComponent<Camera>().orthographicSize < 9)
            {
                gameObject.GetComponent<Camera>().orthographicSize += 0.01f;
            }
        }
    }

    private void Approach(Vector3 vec3)
    {
        //Debug.Log("x - " + objectTransform.position.x + " y - " + objectTransform.position.y + " z - " + objectTransform.position.z);
        Vector3 target = new Vector3()
        {
            x = vec3.x,
            y = vec3.y + y_offset,
            z = -10
        };

        Vector3 pos = Vector3.Lerp(new Vector3(transform.position.x, transform.position.y, -10), target, movingSpeed * Time.deltaTime);
        transform.position = pos;
    }
}
