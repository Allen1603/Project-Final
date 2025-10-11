using System.Collections;
using UnityEngine;

public class EnemyFly : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 2f;
    public float zigzagFrequency = 3f;
    public float zigzagWidth = 1f;
    private float zigzagTimer;

    [Header("Status")]
    public bool isHooked = false;
    //private bool isStunned = false;

    [Header("Visuals")]
    private Renderer modelRenderer;
    private Color originalColor;

    private GameObject player;

    void OnEnable()
    {
        // Reset state for pooling
        isHooked = false;
        //isStunned = false;

        zigzagTimer = 0f;
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");

        if (modelRenderer == null)
            modelRenderer = GetComponentInChildren<Renderer>();

        if (modelRenderer != null)
            modelRenderer.material.color = originalColor;
    }

    void Start()
    {
        modelRenderer = GetComponentInChildren<Renderer>();
        if (modelRenderer != null)
            originalColor = modelRenderer.material.color;

        zigzagTimer = 0f;
    }

    void Update()
    {
        if (isHooked) return;
        if (player == null) return;

        // Zigzag motion
        zigzagTimer += Time.deltaTime * zigzagFrequency;
        float zigzagOffset = Mathf.Sin(zigzagTimer * Mathf.PI * 2) * zigzagWidth;

        transform.position += new Vector3(
            -speed * Time.deltaTime,
            0f,
            zigzagOffset * Time.deltaTime * zigzagFrequency
        );
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hook"))
        {
            isHooked = true;
        }
        if (other.CompareTag("Player"))
        {
            PlayerController.instance.TakeBar(10);
            PlayerController.instance.TakeExp(10);
            EnemyPool.Instance.ReturnToPool("Enemy2", gameObject);
        }
        if (other.CompareTag("Base"))
        {
            EnemyPool.Instance.ReturnToPool("Enemy2", gameObject);
        }
    }

    // --- STATUS EFFECTS BELOW ---

    //public void SlowEffect(float newSpeed, float duration)
    //{
    //    if (!isStunned)
    //    {
    //        float currentSpeed = speed;
    //        speed = newSpeed;
    //        StartCoroutine(BlinkEffect(duration));
    //        StartCoroutine(ResetSpeedAfter(duration, currentSpeed));
    //    }
    //}

//    private IEnumerator ResetSpeedAfter(float duration, float originalSpeed)
//    {
//        yield return new WaitForSeconds(duration);
//        speed = originalSpeed;

//        if (modelRenderer != null)
//            modelRenderer.material.color = originalColor;
//    }

//    private IEnumerator BlinkEffect(float duration)
//    {
//        if (modelRenderer == null) yield break;

//        float elapsed = 0f;
//        bool toggle = false;
//        Color slowColor = Color.blue;

//        while (elapsed < duration)
//        {
//            modelRenderer.material.color = toggle ? slowColor : originalColor;
//            toggle = !toggle;
//            elapsed += 0.2f;
//            yield return new WaitForSeconds(0.2f);
//        }

//        modelRenderer.material.color = originalColor;
//    }

//    public void Stun(float duration)
//    {
//        if (!isStunned)
//            StartCoroutine(StunCoroutine(duration));
//    }

//    private IEnumerator StunCoroutine(float duration)
//    {
//        isStunned = true;

//        float prevSpeed = speed;
//        speed = 0f;

//        yield return new WaitForSeconds(duration);

//        isStunned = false;
//        speed = prevSpeed;
//    }
}
