using UnityEngine;

public static class HandSpawner
{
    private static GameObject handPrefab = Resources.Load<GameObject>("HandPrefab");
    private static float lastSpawnTime = 0f;
    public static float spawnDelay = 2f;

    // Метод для проверки, можно ли спаунить руку
    private static bool CanSpawn()
    {
        return Time.time - lastSpawnTime >= spawnDelay;
    }

    // Обновленный метод для спауна руки
    public static void SpawnHand(Vector3 spawnPosition, float fallSpeed)
    {
        if (!CanSpawn())
        {
            Debug.Log("Hand spawn is on cooldown!");
            return;
        }

        if (handPrefab == null)
        {
            Debug.LogError("HandPrefab not found in Resources folder!");
            return;
        }

        Vector3 adjustedSpawnPosition = new Vector3(spawnPosition.x, spawnPosition.y, 0);
        GameObject handInstance = Object.Instantiate(handPrefab, adjustedSpawnPosition, Quaternion.identity);

        Rigidbody2D rb = handInstance.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = new Vector2(0, -fallSpeed);
        }
        else
        {
            Debug.LogError("HandPrefab missing Rigidbody2D component!");
        }

        lastSpawnTime = Time.time;  // Обновляем время последнего спавна
    }

    public static void SpawnHandDefault(Vector3 spawnPos)
    {
        if (!CanSpawn())
        {
            Debug.Log("Hand spawn is on cooldown!");
            return;
        }
        SpawnHand(spawnPos + new Vector3(0, 2, 0), 10);
    }

    public static void SpawnTrackingHand(Vector3 playerPosition)
    {
        if (!CanSpawn())
        {
            Debug.Log("Hand spawn is on cooldown!");
            return;
        }

        if (handPrefab == null)
        {
            Debug.LogError("HandPrefab not found in Resources folder!");
            return;
        }

        GameObject handInstance = Object.Instantiate(handPrefab, new Vector3(playerPosition.x, playerPosition.y + 2, 0), Quaternion.identity);
        HandBehavior handBehavior = handInstance.GetComponent<HandBehavior>();
        if (handBehavior != null)
        {
            handBehavior.TrackPlayer = true;
        }
        else
        {
            Debug.LogError("HandPrefab missing HandBehavior component!");
        }

        lastSpawnTime = Time.time;  // Обновляем время последнего спавна
    }
}
