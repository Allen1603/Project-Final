using UnityEngine;
using System.Collections;

public class EnemyBug : MonoBehaviour, IStunnable, ISlowable
{
    [Header("Movement Settings")]
    public float speed = 2f;
    public float dashSpeedMultiplier = 2f;
    public float dashDuration = 1f;
    public float dashCooldown = 3f;
    private float baseSpeed;

    private float currentSpeed;               // ← REAL movement speed

    [Header("Status")]
    public bool isHooked = false;
    private bool isDashing = false;
    private bool canDash = true;
    private bool isSlow = false;
    private bool isStunned = false;

    private void OnEnable()
    {
        // Reset states when pulled from pool
        isHooked = false;
        isDashing = false;
        canDash = true;
        isSlow = false;
        isStunned = false;

        baseSpeed = speed;
        currentSpeed = baseSpeed;
    }

    private void Update()
    {
        if (isHooked || isStunned) return;

        // Move left
        transform.position += Vector3.left * currentSpeed * Time.deltaTime;

        // Dash logic
        if (!isDashing && canDash)
            StartCoroutine(DashRoutine());
    }

    // -------------------- DASH -------------------- //
    private IEnumerator DashRoutine()
    {
        canDash = false;
        isDashing = true;

        // Always dash fast (ignores slow)
        currentSpeed = baseSpeed * dashSpeedMultiplier;

        yield return new WaitForSeconds(dashDuration);

        isDashing = false;

        // Return to correct speed
        currentSpeed = isSlow ? baseSpeed * 0.5f : baseSpeed;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    // -------------------- SLOW -------------------- //
    public void SlowEffect(float newSpeed, float duration)
    {
        if (isSlow) return;

        isSlow = true;

        // If dashing, do NOT slow down  
        if (!isDashing)
            currentSpeed = newSpeed;

        StartCoroutine(ResetSlow(duration));
    }

    private IEnumerator ResetSlow(float duration)
    {
        yield return new WaitForSeconds(duration);

        isSlow = false;

        if (!isDashing)
            currentSpeed = baseSpeed;
    }

    // -------------------- STUN -------------------- //
    public void Stun(float duration)
    {
        StartCoroutine(StunCoroutine(duration));
    }

    private IEnumerator StunCoroutine(float duration)
    {
        isStunned = true;
        isDashing = false;
        canDash = false;

        float oldSpeed = currentSpeed;
        currentSpeed = 0;

        yield return new WaitForSeconds(duration);

        isStunned = false;
        canDash = true;

        currentSpeed = isSlow ? baseSpeed * 0.5f : baseSpeed;
    }

    // -------------------- COLLISIONS -------------------- //
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hook"))
            isHooked = true;

        if (other.CompareTag("Player"))
        {
            PlayerController.instance.TakeBar(10f);
            PlayerController.instance.TakeExp(10f);
            EnemyPool.Instance.ReturnToPool("Enemy3", gameObject);
        }

        if (other.CompareTag("Base"))
        {
            EnemyPool.Instance.ReturnToPool("Enemy3", gameObject);
        }
    }
}
