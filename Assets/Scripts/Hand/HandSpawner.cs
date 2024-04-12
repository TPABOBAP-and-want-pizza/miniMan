using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public static class HandSpawner
{
    // Загружаем префаб руки
    private static GameObject handPrefab = Resources.Load<GameObject>("HandPrefab");

    // Метод для спауна руки
    public static void SpawnHand(Vector3 spawnPosition, float fallSpeed)
    {
        if (handPrefab == null)
        {
            Debug.LogError("HandPrefab not found in Resources folder!");
            return;
        }

        // Создаем экземпляр руки
        GameObject handInstance = Object.Instantiate(handPrefab, spawnPosition, Quaternion.identity);

        // Настройка начальной скорости падения руки
        Rigidbody2D rb = handInstance.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = new Vector2(0, -fallSpeed);
        }
        else
        {
            Debug.LogError("HandPrefab missing Rigidbody2D component!");
        }
    }

    public static void SpawnHandDefault(Vector3 spawnPos)
    {
        
        Vector3 spawnPosition = (spawnPos + new Vector3(0, 20, 11));


        SpawnHand(spawnPosition, 10);
    }
}
