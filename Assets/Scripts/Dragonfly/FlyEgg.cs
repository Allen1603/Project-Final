using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEgg : MonoBehaviour
{
    private float eggHatchingInterval;
    public GameObject nymphsPrefab;
    void Update()
    {
        StartCoroutine(EggHatching());
    }

    IEnumerator EggHatching()
    {
        eggHatchingInterval = Random.Range(5,10);
        yield return new WaitForSeconds(eggHatchingInterval);
        EnemyPool.Instance.SpawnFromPool("Enemy4", transform.position, Quaternion.identity);
        EnemyPool.Instance.ReturnToPool("Egg", gameObject);
    }

}
