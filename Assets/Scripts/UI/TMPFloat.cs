using UnityEngine;
using TMPro;

public class TMPFloat : MonoBehaviour
{
    [Header("Float Settings")]
    public float floatAmplitude = 20f;   // Height of up & down movement
    public float floatSpeed = 2f;        // Speed of movement
    public bool playOnStart = true;

    private Vector3 startPos;
    private bool isFloating = false;
    private float timeCounter = 0f;

    private void Awake()
    {
        startPos = transform.localPosition;
    }

    private void Start()
    {
        if (playOnStart)
            isFloating = true;
    }

    private void Update()
    {
        if (!isFloating) return;

        timeCounter += Time.unscaledDeltaTime * floatSpeed;

        // Smooth up and down motion using sin wave
        float offsetY = Mathf.Sin(timeCounter) * floatAmplitude;

        transform.localPosition = new Vector3(
            startPos.x,
            startPos.y + offsetY,
            startPos.z
        );
    }

    public void StartFloating()
    {
        isFloating = true;
        timeCounter = 0f;
    }

    public void StopFloating()
    {
        isFloating = false;
        transform.localPosition = startPos;
    }
}
