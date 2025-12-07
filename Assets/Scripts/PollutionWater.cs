using System.Collections;
using UnityEngine;

public class PollutionWater : MonoBehaviour
{
    public float waterSpeed = 2f;
    public float zigzagFrequency = 2f;
    public float zigzagWidth = 1f;
    private float zigzagTimer;

    private void Start()
    {
        //transform.position = new Vector3(7.16f, 0.55f, 5.43f);

    }
    private void Update()
    {
        // ---- ALWAYS MOVE LEFT ---- //
        Vector3 forwardMove = Vector3.left * waterSpeed * Time.deltaTime;

        // ---- ZIGZAG ---- //
        zigzagTimer += Time.deltaTime * zigzagFrequency;
        float zigzagOffset = Mathf.Sin(zigzagTimer * Mathf.PI * 2) * zigzagWidth;

        Vector3 zigzagMove = new Vector3(0f, 0f, zigzagOffset * Time.deltaTime);

        transform.position += forwardMove + zigzagMove;
    }
}
