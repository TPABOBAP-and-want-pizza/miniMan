using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, Receiver
{
    [SerializeField] List<Transform> trTransmitters = new List<Transform>();
    private List<Transmitter> transmitters = new List<Transmitter>();
    [SerializeField] int shouldActivates = 1;
    private int countActivate = 0;
    private BoxCollider2D collider2D = null;
    public bool activate;

    private void Start()
    {
        collider2D = transform.GetComponent<BoxCollider2D>();

        foreach (Transform i in trTransmitters)
        {
            transmitters.Add(i.GetComponent<Transmitter>());
        }

        foreach (Transmitter i in transmitters)
        {
            i.OnActivate += OnActivation;
            i.OnDeactivate += OnDeactivation;
        }
    }

    private void CheckActivity()
    {
        if (countActivate >= shouldActivates)
        {
            activate = true;
        }
        else activate = false;

        if (activate)
        {
            if(collider2D != null && collider2D.enabled == true)
            {
                collider2D.enabled = false;
            }
        }
        else
        {
            if (collider2D != null && collider2D.enabled == false)
            {
                collider2D.enabled = true;
            }
        }

    }

    public void OnActivation()
    {
        ++countActivate;
        CheckActivity();
    }

    public void OnDeactivation()
    {
        countActivate--;
        CheckActivity();
    }

    private void OnDisable()
    {
        foreach (Transmitter i in transmitters)
        {
            i.OnActivate -= OnActivation;
            i.OnDeactivate -= OnDeactivation;
        }
    }
}
