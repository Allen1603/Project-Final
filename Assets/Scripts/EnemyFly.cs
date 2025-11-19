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
    private bool isStunned = false;

    void OnEnable()
    {
        // Reset state for pooling
        isHooked = false;
        isStunned = false;

        zigzagTimer = 0f;
    }

    void Start()
    {
        zigzagTimer = 0f;
    }

    void Update()
    {
        if (isHooked) return;

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
            PlayerController.instance.TakeBar(10f);
            PlayerController.instance.TakeExp(10f);
            EnemyPool.Instance.ReturnToPool("Enemy2", gameObject);
        }
        if (other.CompareTag("Base"))
        {
            EnemyPool.Instance.ReturnToPool("Enemy2", gameObject);
        }
    }

    // --- STATUS EFFECTS BELOW ---

    public void SlowEffect(float newSpeed, float duration)
    {
        if (!isStunned)
        {
            float currentSpeed = speed;
            speed = newSpeed;
            StartCoroutine(ResetSpeedAfter(duration, currentSpeed));
        }
    }

    private IEnumerator ResetSpeedAfter(float duration, float originalSpeed)
    {
        yield return new WaitForSeconds(duration);
        speed = originalSpeed;
    }

    //----------Stun--------
    public void Stun(float duration)
    {
        if (!isStunned)
            StartCoroutine(StunCoroutine(duration));
    }

    private IEnumerator StunCoroutine(float duration)
    {
        isStunned = true;

        float prevSpeed = speed;
        speed = 0f;

        yield return new WaitForSeconds(duration);

        isStunned = false;
        speed = prevSpeed;
    }
}
