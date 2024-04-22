using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerDeath : MonoBehaviour
{
    public event Action respawn;   
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private ShowDeathScreen deathScreen;
    private Movement movement;

    private Rigidbody2D body;

    [SerializeField] float respawnDelay = 0.5f;
    private bool isDead = false;
    private bool canResapwn = false;

    private void Start()
    {
        movement = GetComponent<Movement>();
        body = transform.GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject go = collision?.gameObject;

        if ((go.layer == 9 || go.tag == "Hand" ||(go.tag == "Cockroach" && !go.GetComponent<CockroachAI>().isDead)) && !isDead)  //9 = player death
        {
            isDead = true;
            StartCoroutine(CallMethodAfterDelay(respawnDelay));
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

        if ((go.layer == 9 || go.tag == "Hand" || (go.tag == "Cockroach" && !go.GetComponent<CockroachAI>().isDead)) && !isDead)  //9 = player death
        {
            isDead = true;
            StartCoroutine(CallMethodAfterDelay(respawnDelay));
            deathScreen.Die(false);
        }
        else if(go.tag == "Checkpoint")
        {
            respawnPoint = go.transform;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Respawn();
        }
        if (isDead && movement.enabled == true)
        {
            movement.enabled = false;
        }
        if (isDead && Input.anyKeyDown && canResapwn)
        {
            canResapwn = false;
            Respawn();
        }

        if (!isDead && transform.position.y < -100)
        {
            isDead = true;
            deathScreen.Die(true);
        }
    }

    public void Respawn()
    {
        isDead = false;

        GetComponent<Movement>().enabled = true;

        transform.position = respawnPoint.position;
        NoiseLevel.Instance.Reset();
        respawn.Invoke();
        DestroyHands();
        deathScreen.HideDeath();
    }

    private void DestroyHands()
    {
        HandDestroyer[] hands = FindObjectsOfType<HandDestroyer>();

        foreach(HandDestroyer i in hands)
        {
            i.DestroyHand();
        }
    }

    IEnumerator CallMethodAfterDelay(float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);
        deathScreen.ShowPressKey();
        SetCanRespawnTrue();
    }

    void SetCanRespawnTrue()
    {
        canResapwn = true;
    }
}
