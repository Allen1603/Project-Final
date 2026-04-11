using System.Collections;
using UnityEngine;

public class FlyEgg : MonoBehaviour
{
    private Coroutine hatchRoutine;

    private void OnEnable()
    {
        hatchRoutine = StartCoroutine(EggHatching());
    }

    private void OnDisable()
    {
        if (hatchRoutine != null)
            StopCoroutine(hatchRoutine);
    }

    private IEnumerator EggHatching()
    {
        float hatchTime = Random.Range(5f, 10f);

        // 🔥 IMPORTANT: use realtime (so it works even if paused)
        yield return new WaitForSecondsRealtime(hatchTime);

        // 🔥 SPAWN NYMPH
        GameObject nymph = EnemyPool.Instance.SpawnFromPool(
            "Enemy4", // nymph tag
            transform.position,
            Quaternion.identity
        );

        // ✅ REGISTER THIS ENEMY (ENTRY COUNT FIX)
        if (NewSpawnerEnemy.Instance != null)
        {
            NewSpawnerEnemy.Instance.RegisterEnemy();
        }

        // 🥚 REMOVE EGG (NOT COUNTED AS ENEMY)
        EnemyPool.Instance.ReturnToPool("Egg", gameObject);
    }
}