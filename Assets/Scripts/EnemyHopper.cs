using System.Collections;
using UnityEngine;

public class EnemyHopper : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 2f;
    public float jumpHeight = 1f;
    public float jumpDuration = 0.5f;
    public float jumpInterval = 3f;

    private float jumpTimer;
    private bool isJumping = false;
    private float jumpStartTime;
    private Vector3 jumpStartPos;
    private float originalSpeed;
    private bool isStunned = false;

    public bool isHooked = false;

    void Start()
    {
        jumpTimer = jumpInterval;
    }

    void Update()
    {
        if (isHooked) return;

        // Move left
        transform.position += Vector3.left * speed * Time.deltaTime;

        // Handle jumping
        jumpTimer -= Time.deltaTime;
        if (!isJumping && jumpTimer <= 0f)
        {
            isJumping = true;
            jumpStartTime = Time.time;
            jumpStartPos = transform.position;
            jumpTimer = jumpInterval;
        }

        if (isJumping)
        {
            float elapsed = Time.time - jumpStartTime;
            float percent = elapsed / jumpDuration;

            if (percent >= 1f)
            {
                isJumping = false;
                transform.position = new Vector3(transform.position.x, jumpStartPos.y, transform.position.z);
            }
            else
            {
                float yOffset = Mathf.Sin(percent * Mathf.PI) * jumpHeight;
                transform.position = new Vector3(transform.position.x, jumpStartPos.y + yOffset, transform.position.z);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hook"))
        {
            isHooked = true;
        }
        if (other.CompareTag("Player"))
        {
            PlayerController.instance.TakeBar(10f);
            PlayerController.instance.TakeExp(10f);
            EnemyPool.Instance.ReturnToPool("Enemy1", gameObject);
        }
        if (other.CompareTag("Base"))
        {
            EnemyPool.Instance.ReturnToPool("Enemy1", gameObject);
        }
    }

    // Slow Effect
    public void SlowEffect(float newSpeed, float duration)
    {
        if (isStunned) return;
        StopCoroutine(nameof(SlowCoroutine));
        StartCoroutine(SlowCoroutine(newSpeed, duration));
    }

    private IEnumerator SlowCoroutine(float newSpeed, float duration)
    {
        float prevSpeed = speed;
        speed = newSpeed;

        yield return new WaitForSeconds(duration);

        speed = originalSpeed;
    }

    // Stun logic
    public void Stun(float duration)
    {
        if (!isStunned)
        {
            StopAllCoroutines(); // Stop movement-related effects
            StartCoroutine(StunCoroutine(duration));
        }
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
