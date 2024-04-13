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

        // Устанавливаем z-координату на 0
        Vector3 adjustedSpawnPosition = new Vector3(spawnPosition.x, spawnPosition.y, 0);

        // Создаем экземпляр руки
        GameObject handInstance = Object.Instantiate(handPrefab, adjustedSpawnPosition, Quaternion.identity);

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
        SpawnHand(spawnPos + new Vector3(0, 17, 0), 10); //17 это высота появления руки, 10 это начальная скорость 
    }
}
