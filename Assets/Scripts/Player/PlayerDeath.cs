using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    public event Action respawn;   
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private ShowDeathScreen deathScreen;
    private Movement movement;

    private bool isDead = false;

    private void Start()
    {
        movement = GetComponent<Movement>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject go = collision?.gameObject;

        if ((go.layer == 9 || go.tag == "Hand") && !isDead)  //9 = player death
        {
            isDead = true;
            deathScreen.Die(false);
        }
        else if (go.tag == "CheckPoint")
        {
            respawnPoint = go.transform;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject go = collision?.gameObject;

        if ((go.layer == 9 || go.tag == "Hand") && !isDead)  //9 = player death
        {
            isDead = true;
            deathScreen.Die(false);
        }
        else if(go.tag == "Checkpoint")
        {
            respawnPoint = go.transform;
        }
    }

    private void Update()
    {
        if (isDead && movement.enabled == true)
        {
            movement.enabled = false;
        }
        if (isDead && Input.anyKeyDown)
        {
            Respawn();
        }

        if (!isDead && transform.position.y < -100)
        {
            isDead = true;
            deathScreen.Die(true);
        }
    }

    private void Respawn()
    {
        isDead = false;

        GetComponent<Movement>().enabled = true;

        transform.position = respawnPoint.position;
        respawn.Invoke();
        deathScreen.HideDeath();
    }
}
