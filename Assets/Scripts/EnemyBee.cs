using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBee : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 2f;
    public float chargeSpeed = 10f;
    public float detectionRange = 5f;
    public float chargeDelay = 1f;

    private GameObject player;
    private float chargeTimer = 2f;
    public bool isHooked = false;
    private bool isAttacking = false;
    private bool isStunned = false;
    private bool isSlow = false;
    private float originalSpeed;
    private float originalChargeSpeed;

    void OnEnable()
    {
        isHooked = false;
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        originalSpeed = speed;
        originalChargeSpeed = chargeSpeed;
    }

    void Update()
    {
        if (isHooked) return;
        if (player == null) return;

        chargeTimer -= Time.deltaTime;

        if (chargeTimer >= 0f)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.left);

            // Move left
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
        else
        {
            if (!isStunned)
            {
                isAttacking = true;

                FaceTarget();

                Vector3 direction = (player.transform.position - transform.position).normalized;
                transform.position += direction * chargeSpeed * Time.deltaTime;
            }
        }
    }

    void FaceTarget()
    {
        if (player == null) return;

        // Direction from clone to target
        Vector3 direction = player.transform.position - transform.position;
        direction.y = 0f; // optional if you want to ignore vertical rotation

        if (direction != Vector3.zero)
        {
            // Instantly face the target
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hook"))
        {
            isHooked = true;
            PlayerController.instance.TakeBar(10f);
            PlayerController.instance.TakeExp(10f);
        }
        if (other.CompareTag("Player"))
        {
            EnemyPool.Instance.ReturnToPool("Enemy4", gameObject);

            if (isAttacking && !isHooked)
            {
                PlayerController.instance.TakeDamage(10f);
            }
        }

        if (other.CompareTag("Base"))
        {
            EnemyPool.Instance.ReturnToPool("Enemy4", gameObject);
        }
    }

    // -------------------- STATUS EFFECTS --------------------//

    // -------------------- SLOW --------------------//
    public void SlowEffect(float newSpeed, float slowDuration)
    {
        if (!isSlow)
        {
            originalSpeed = speed;
            originalChargeSpeed = chargeSpeed;
        }

        isSlow = true;
        speed = newSpeed;
        chargeSpeed = newSpeed;   // ← SLOW the charge too

        StartCoroutine(ResetSlow(slowDuration));
    }

    private IEnumerator ResetSlow(float duration)
    {
        yield return new WaitForSeconds(duration);

        speed = originalSpeed;
        chargeSpeed = originalChargeSpeed;

        isSlow = false;
    }

    // -------------------- STUN --------------------//
    public void Stun(float stunDuration)
    {
        StartCoroutine(StunCoroutine(stunDuration));
        
    }

    private IEnumerator StunCoroutine(float duration)
    {
        isStunned = true;
        float prevSpeed = speed;
        speed = 0f; // stop movement

        yield return new WaitForSeconds(duration);

        // Restore
        speed = prevSpeed;
        isStunned = false;
    }
}
