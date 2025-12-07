using UnityEngine;

public class PollutionWater : MonoBehaviour
{
    [Header("Movement")]
    public float waterSpeed = 2f;
    public float zigzagFrequency = 2f;
    public float zigzagWidth = 1f;
    private float zigzagTimer;
 

    private void Update()
    {
        MovePollution();
    }

    void MovePollution()
    {
        // Move left
        Vector3 forwardMove = Vector3.left * waterSpeed * Time.deltaTime;

        // Zigzag
        zigzagTimer += Time.deltaTime * zigzagFrequency;
        float zigzagOffset = Mathf.Sin(zigzagTimer * Mathf.PI * 2) * zigzagWidth;

        Vector3 zigzagMove = new Vector3(0f, 0f, zigzagOffset * Time.deltaTime);

        transform.position += forwardMove + zigzagMove;
    }
}
