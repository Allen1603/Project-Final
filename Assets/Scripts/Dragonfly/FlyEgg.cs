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
        yield return new WaitForSeconds(hatchTime);

        // Spawn nymph (REAL enemy → must have EnemyBase)
        EnemyPool.Instance.SpawnFromPool(
            "Enemy4", // nymph tag
            transform.position,
            Quaternion.identity
        );

        // Remove egg (NOT an enemy)
        EnemyPool.Instance.ReturnToPool("Egg", gameObject);
    }
}
