using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject deadScreen;

    private bool isDead = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject go = collision?.gameObject;

        if (go.layer == 9 && !isDead)  //9 = player death
        {
            Die();
        }
        else if (go.tag == "CheckPoint")
        {
            respawnPoint = go.transform;
        }
    }

    private void Die()
    {
        isDead = true;
        deadScreen.active = true;

        GetComponent<Movement>().enabled = false;

        //animator.Play("Death");

        ShowDeathScreen();
    }

    private void ShowDeathScreen()
    {
        Debug.Log("You Dead");
        deadScreen.active = true;
    }

    private void Update()
    {
        if (isDead && Input.anyKeyDown)
        {
            Respawn();
        }

        if (!isDead && transform.position.y < -100)
        {
            Die();
        }
    }

    private void Respawn()
    {
        isDead = false;

        GetComponent<Movement>().enabled = true;

        transform.position = respawnPoint.position;
        deadScreen.active = false;
    }
}
