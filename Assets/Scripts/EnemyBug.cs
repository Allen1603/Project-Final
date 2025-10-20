using UnityEngine;
using System.Collections;

public class EnemyBug : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 2f;                  // Normal movement speed
    public float dashSpeedMultiplier = 2f;    // Speed multiplier during dash
    public float dashDuration = 1f;           // How long dash lasts
    public float dashCooldown = 3f;           // Time between dashes

    [Header("Status")]
    //private bool isStunned = false;
    //private float originalSpeed;

    //[Header("Visuals")]
    //private Renderer modelRenderer;
    //private Color originalColor;

    //[Header("Effects")]
    //public GameObject stunEffectPrefab;
    //private GameObject stunEffectInstance;

    public bool isHooked = false;
    //private GameObject player;

    private bool isDashing = false;
    private bool canDash = true;

    void Update()
    {
        if (isHooked) return; // Stop moving if hooked

        // Move left continuously
        transform.position += Vector3.left * speed * Time.deltaTime;

        // Start dash if allowed
        if (canDash && !isDashing)
            StartCoroutine(DashRoutine());
    }

    // -------------------- DASH LOGIC --------------------
    private IEnumerator DashRoutine()
    {
        canDash = false;       // prevent new dashes
        isDashing = true;      // currently dashing

        float originalSpeed = speed;
        speed = originalSpeed * dashSpeedMultiplier;  // speed boost
        yield return new WaitForSeconds(dashDuration);

        speed = originalSpeed; // reset to normal
        isDashing = false;

        // cooldown before next dash
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

    // -------------------- STATUS EFFECTS --------------------
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

    //private IEnumerator ResetSpeedAfter(float duration, float originalSpeed)
    //{
    //    yield return new WaitForSeconds(duration);
    //    speed = originalSpeed;

    //    if (modelRenderer != null)
    //        modelRenderer.material.color = originalColor;
    //}

    //private IEnumerator BlinkEffect(float duration)
    //{
    //    if (modelRenderer == null) yield break;

    //    float elapsed = 0f;
    //    bool toggle = false;
    //    Color slowColor = Color.blue;

    //    while (elapsed < duration)
    //    {
    //        modelRenderer.material.color = toggle ? slowColor : originalColor;
    //        toggle = !toggle;
    //        elapsed += 0.2f;
    //        yield return new WaitForSeconds(0.2f);
    //    }

    //    modelRenderer.material.color = originalColor;
    //}

    //// -------------------- STUN --------------------
    //public void Stun(float duration)
    //{
    //    if (!isStunned)
    //        StartCoroutine(StunCoroutine(duration));
    //}

    //private IEnumerator StunCoroutine(float duration)
    //{
    //    isStunned = true;

    //    float prevSpeed = speed;
    //    speed = 0f; // stop movement

    //    // Spawn stun effect
    //    if (stunEffectPrefab != null && stunEffectInstance == null)
    //    {
    //        stunEffectInstance = Instantiate(stunEffectPrefab, transform.position + Vector3.up * 1f, Quaternion.identity);
    //        stunEffectInstance.transform.SetParent(transform);
    //    }

    //    yield return new WaitForSeconds(duration);

    //    // Restore
    //    isStunned = false;
    //    speed = prevSpeed;

    //    if (stunEffectInstance != null)
    //    {
    //        Destroy(stunEffectInstance);
    //        stunEffectInstance = null;
    //    }
    //}
}
