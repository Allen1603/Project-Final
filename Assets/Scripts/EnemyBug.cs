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
    //private bool isStunned = false;


    private void Start()
    {
        isHooked = false;
        baseSpeed = speed;
    }
    void Update()
    {
        if (isHooked) return;

        // Move Left
        transform.position += Vector3.left * speed * Time.deltaTime;

        // Dash logic
        if (!isDashing && canDash)
            StartCoroutine(DashRoutine());
    }

    private IEnumerator DashRoutine()
    {
        canDash = false;
        isDashing = true;

        float dashSpeed = baseSpeed * dashSpeedMultiplier;

        // Start dash
        if (!isSlow)
            speed = dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        // End dash
        if (!isSlow)
            speed = baseSpeed;

        isDashing = false;

        // Cooldown
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

        // If dashing, keep dash speed
        if (!isDashing)
            speed = baseSpeed;
    }



    // -------------------- STUN --------------------//
    public void Stun(float stunDuration)
    {
        StartCoroutine(StunCoroutine(stunDuration));
    }

    private IEnumerator StunCoroutine(float duration)
    {

        speed = 0f;       // full stop
        isDashing = false; // cancel dash
        canDash = false;   // disable future dashes

        yield return new WaitForSeconds(duration);

        canDash = true;

        // Restore correct speed after stun
        speed = isSlow ? speed : baseSpeed;
    }
}
