using UnityEngine;

public class RotatingIcon : MonoBehaviour
{
    public float speed = 100f;

    void Update()
    {
        // Clockwise rotation (negative Z)
        transform.Rotate(0f, 0f, -speed * Time.deltaTime);
    }
}
