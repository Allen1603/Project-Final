using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEgg : MonoBehaviour
{
    public float eggHatchingInterval = 2f;
    public GameObject nymphsPrefab;
    void Update()
    {
        StartCoroutine(EggHatching());
    }

    IEnumerator EggHatching()
    {
        yield return new WaitForSeconds(eggHatchingInterval);
        EnemyPool.Instance.SpawnFromPool("Enemy4", transform.position, Quaternion.identity);
        EnemyPool.Instance.ReturnToPool("Egg", gameObject);
    }

}
