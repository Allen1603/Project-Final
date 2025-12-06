using UnityEngine;

public class EggManager : MonoBehaviour
{
    //public GameObject eggPrefab;
    //public int eggCount = 50;

    //private int eggsAlive;

    //private void Start()
    //{
    //    eggsAlive = eggCount;
    //    SpawnEggs();
    //}

    //void SpawnEggs()
    //{
    //    for (int i = 0; i < eggCount; i++)
    //    {
    //        // position eggs near each other randomly
    //        Vector3 pos = transform.position + new Vector3(
    //            Random.Range(-1f, 1f),
    //            0,
    //            Random.Range(-1f, 1f)
    //        );

    //        GameObject egg = Instantiate(eggPrefab, pos, Quaternion.identity);

    //        // Tell egg who the manager is
    //        egg.GetComponent<EggHealth>().manager = this;
    //    }
    //}

    //public void EggDied()
    //{
    //    eggsAlive--;

    //    if (eggsAlive <= 0)
    //    {
    //        Debug.Log("Player LOST! All eggs died.");
    //        // trigger game over screen
    //    }
    //}
}
