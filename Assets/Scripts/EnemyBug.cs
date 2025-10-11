using UnityEngine;
using System.Collections;

public class EnemyBug : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 2f;                  // Normal movement speed
    public float dashSpeedMultiplier = 3f;    // Speed multiplier during dash
    public float dashDuration = 1f;           // How long dash lasts
    public float dashCooldown = 3f;           // Time between dashes

    [Header("Status")]
    //private bool isStunned = false;
    private float originalSpeed;

    //[Header("Visuals")]
    //private Renderer modelRenderer;
    //private Color originalColor;

    //[Header("Effects")]
    //public GameObject stunEffectPrefab;
    //private GameObject stunEffectInstance;

    public bool isHooked = false;
    private Coroutine dashCoroutine;
    //private GameObject player;

    // -------------------- START --------------------
    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player");
        originalSpeed = speed;

        //modelRenderer = GetComponentInChildren<Renderer>();
        //if (modelRenderer != null)
        //    originalColor = modelRenderer.material.color;

        dashCoroutine = StartCoroutine(DashRoutine());
    }

    void Update()
    {
        if (isHooked) return; // Skip movement if stunned or hooked
        //if (player == null) return;

        // Normal movement (left) — simple dash speed handled by coroutine
        transform.position += Vector3.left * speed * Time.deltaTime;
    }

    // -------------------- DASH LOGIC --------------------
    IEnumerator DashRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(dashCooldown);
            yield return StartCoroutine(BugDashSkill());
        }
    }

    IEnumerator BugDashSkill()
    {
        speed *= dashSpeedMultiplier;
        yield return new WaitForSeconds(dashDuration);
        speed = originalSpeed;
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
            PlayerController.instance.TakeBar(10);
            PlayerController.instance.TakeExp(10);
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

    // -------------------- CLEANUP --------------------
    private void OnDisable()
    {
        if (dashCoroutine != null)
        {
            StopCoroutine(dashCoroutine);
            dashCoroutine = null;
        }
    }
}
