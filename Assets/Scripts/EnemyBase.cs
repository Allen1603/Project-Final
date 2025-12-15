using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    protected virtual void OnEnable()
    {
        // Count ONLY real enemies
        if (SpawnerEnemy.Instance != null)
            SpawnerEnemy.Instance.RegisterEnemy();
    }

    protected virtual void OnDisable()
    {
        // Remove ONLY real enemies
        if (SpawnerEnemy.Instance != null)
            SpawnerEnemy.Instance.UnregisterEnemy();
    }
}
