using UnityEngine;

public class SimpleWindSway : MonoBehaviour
{
    public float swayStrength = 1f;
    public float swaySpeed = 1f;
    public float swayAngle = 10f;

    private Quaternion startRot;

    void Start()
    {
        startRot = transform.localRotation;
    }

    void Update()
    {
        float sway = Mathf.Sin(Time.time * swaySpeed) * swayAngle * swayStrength;
        transform.localRotation = startRot * Quaternion.Euler(0, sway, 0);
    }
}