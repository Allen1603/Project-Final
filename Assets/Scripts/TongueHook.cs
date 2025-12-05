using UnityEngine;

public class TongueHook : MonoBehaviour
{
    public float tongueSpeed = 0f;
    public float tongueReturnSpeed = 0f;
    public float tongueRange = 0f;

    [Header("Tags this tongue can catch")]
    public string[] enemyTagInsect;

    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < enemyTagInsect.Length; i++)
        {
            if (other.CompareTag(enemyTagInsect[i]))
            {
                CatchEnemy(other.gameObject);
                return;
            }
        }
    }

    private void CatchEnemy(GameObject enemy)
    {
        
    }
}
