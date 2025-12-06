using System.Collections;
using UnityEngine;

public class BeetleBoss : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 1.5f;
    private float baseSpeed;
    private float currentSpeed;

    [Header("Health")]
    public float BossHealth = 300f;
    private float currentBossHealth;

    [Header("Combat")]
    public float tongueDamage = 20f;
    public bool isHooked = false;

    [Header("Passive Immunity")]
    public float immunityDuration = 3f;      // IMMUNE for 3 seconds
    public float minCooldown = 2f;           // activates every 2–3 sec
    public float maxCooldown = 3f;
    public bool isImmune = false;

    // ---- Status Effect Flags ----
    private bool isSlow = false;
    private bool isStunned = false;
    public GameObject shieldVFX;

    private void OnEnable()
    {
        isHooked = false;
        currentBossHealth = BossHealth;

        baseSpeed = moveSpeed;
        currentSpeed = moveSpeed;

        isImmune = false;
        StartCoroutine(PassiveImmunityRoutine());
    }

    private void Update()
    {
        if (isHooked || isStunned) return;

        transform.position += Vector3.left * currentSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    // ====== Passive Immunity ======
    private IEnumerator PassiveImmunityRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(minCooldown, maxCooldown);
            yield return new WaitForSeconds(waitTime);

            // Enable IMMUNITY
            isImmune = true;
            shieldVFX.SetActive(true);

            yield return new WaitForSeconds(immunityDuration);

            // Disable IMMUNITY
            isImmune = false;
            shieldVFX.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hook"))
        {
            TDamage(tongueDamage);
        }

        if (other.CompareTag("Base"))
        {
            EnemyPool.Instance.ReturnToPool("Boss2", gameObject);
        }
    }

    // ===================== SLOW =====================
    public void SlowEffect(float newSpeed, float duration)
    {
        if (isImmune) return;   // ❌ BLOCKED BY IMMUNITY
        if (isSlow) return;

        isSlow = true;

        currentSpeed = newSpeed;

        StartCoroutine(ResetSlow(duration));
    }

    private IEnumerator ResetSlow(float duration)
    {
        yield return new WaitForSeconds(duration);

        isSlow = false;

        currentSpeed = baseSpeed;
    }

    // ===================== STUN =====================
    public void Stun(float duration)
    {
        if (isImmune) return;   // ❌ BLOCKED BY IMMUNITY
        StartCoroutine(StunCoroutine(duration));
    }

    private IEnumerator StunCoroutine(float duration)
    {
        isStunned = true;

        float oldSpeed = currentSpeed;
        currentSpeed = 0;

        yield return new WaitForSeconds(duration);

        isStunned = false;

        currentSpeed = isSlow ? baseSpeed * 0.5f : baseSpeed;
    }

    // ===================== DAMAGE =====================
    public void TDamage(float amount)
    {

        currentBossHealth -= amount;

        if (currentBossHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isHooked = true;
        PlayerController.instance.TakeBar(100f);
        PlayerController.instance.TakeExp(100f);

        EnemyPool.Instance.ReturnToPool("Boss2", gameObject);
    }
}
