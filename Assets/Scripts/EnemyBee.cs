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
            Vector3 direction = (player.transform.position - transform.position).normalized;
            transform.position += direction * chargeSpeed * Time.deltaTime;
        }
    }

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
            EnemyPool.Instance.ReturnToPool("Enemy4", gameObject);
        }
        if (other.CompareTag("Base"))
        {
            EnemyPool.Instance.ReturnToPool("Enemy4", gameObject);
        }
    }

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

    //public void Stun(float duration)
    //{
    //    if (!isStunned)
    //        StartCoroutine(StunCoroutine(duration));
    //}

    //private IEnumerator StunCoroutine(float duration)
    //{
    //    isStunned = true;
    //    float savedSpeed = speed;
    //    speed = 0f;

    //    yield return new WaitForSeconds(duration);

    //    isStunned = false;
    //    speed = savedSpeed;
    //}
}
