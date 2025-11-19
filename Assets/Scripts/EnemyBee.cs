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
    private float originalSpeed;
    private bool isStunned = false;

    //private float originalSpeed;

    void OnEnable()
    {
        isHooked = false;
        //speed = originalSpeed;
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //originalSpeed = speed;
    }

    void Update()
    {
        if (isHooked) return;
        if (player == null) return;

        chargeTimer -= Time.deltaTime;
        if (chargeTimer >= 0f)
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
        else
        {
            isAttacking = true;
            Vector3 direction = (player.transform.position - transform.position).normalized;
            transform.position += direction * chargeSpeed * Time.deltaTime;
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

    public void SlowEffect(float newSpeed, float duration)
    {
        if (!isStunned)
        {
            float currentSpeed = speed;
            speed = newSpeed;
            StartCoroutine(ResetSpeedAfter(duration, currentSpeed));
            speed = originalSpeed;
        }
    }

    private IEnumerator ResetSpeedAfter(float duration, float originalSpeed)
    {
        yield return new WaitForSeconds(duration);
        speed = originalSpeed;
    }

    public void Stun(float duration)
    {
        if (!isStunned)
            StartCoroutine(StunCoroutine(duration));
    }

    private IEnumerator StunCoroutine(float duration)
    {
        isStunned = true;
        float savedSpeed = speed;
        speed = 0f;

        yield return new WaitForSeconds(duration);

        isStunned = false;
        speed = savedSpeed;
    }
}
