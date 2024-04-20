using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField] PlayerDeath playerDeath;
    [SerializeField] Rigidbody2D body;
    Vector3 respawnPos;
    void Start()
    {
        respawnPos = transform.position;
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        playerDeath = playerObject?.GetComponent<PlayerDeath>();
        playerDeath.respawn += RespawnObj;
    }

    private void RespawnObj()
    {
        if(body != null)
            body.velocity = Vector2.zero;

        transform.position = respawnPos;
        CockroachAI cockroach = gameObject.GetComponent<CockroachAI>();
        cockroach?.Alive();
    }

    private void OnDisable()
    {
        playerDeath.respawn -= RespawnObj;
    }
}
