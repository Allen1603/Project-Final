using UnityEngine;
using System.Collections;

public class EnemyBug : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 2f;                  // Normal movement speed
    public float dashSpeedMultiplier = 2f;    // Speed multiplier during dash
    public float dashDuration = 1f;           // How long dash lasts
    public float dashCooldown = 3f;           // Time between dashes
    private float baseSpeed;

    [Header("Status")]
    public bool isHooked = false;
    private bool isDashing = false;
    private bool canDash = true;
    private bool isSlow = false;
    private bool isStunned = false;

    private void Start()
    {
        baseSpeed = speed;
    }
    void Update()
    {
        if (isHooked) return;
        if (isStunned) return;  // ⛔ STOP ALL MOVEMENT

        transform.position += Vector3.left * speed * Time.deltaTime;

        if (canDash && !isDashing)
            StartCoroutine(DashRoutine());
    }

    // -------------------- DASH LOGIC --------------------
    private IEnumerator DashRoutine()
    {
        canDash = false;
        isDashing = true;

        // If stunned, abort dash immediately
        if (isStunned)
        {
            isDashing = false;
            canDash = true;
            yield break;
        }

        float dashSpeed = baseSpeed * dashSpeedMultiplier;

        speed = isSlow ? speed : dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        speed = isSlow ? speed : baseSpeed;

        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }



    // -------------------- COLLISIONS --------------------
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
            EnemyPool.Instance.ReturnToPool("Enemy3", gameObject);
        }
        if (other.CompareTag("Base"))
        {
            EnemyPool.Instance.ReturnToPool("Enemy3", gameObject);
        }
    }

    // -------------------- STATUS EFFECTS --------------------//

    // -------------------- SLOW --------------------//
    public void SlowEffect(float newSpeed, float slowDuration)
    {
        if (!isSlow)
        {
            isSlow = true;
            speed = newSpeed;
            StartCoroutine(ResetSlow(slowDuration));
        }
    }
    private IEnumerator ResetSlow(float duration)
    {
        yield return new WaitForSeconds(duration);
        isSlow = false;

        // restore to base speed, but if dashing, let dash handle speed
        if (!isDashing)
        {
            speed = baseSpeed;
        }
    }


    // -------------------- STUN --------------------//
    public void Stun(float stunDuration)
    {
        StartCoroutine(StunCoroutine(stunDuration));
    }

    private IEnumerator StunCoroutine(float duration)
    {
        isStunned = true;

        speed = 0f;       // full stop
        isDashing = false; // cancel dash
        canDash = false;   // disable future dashes

        yield return new WaitForSeconds(duration);

        isStunned = false;
        canDash = true;

        // Restore correct speed after stun
        speed = isSlow ? speed : baseSpeed;
    }

}
