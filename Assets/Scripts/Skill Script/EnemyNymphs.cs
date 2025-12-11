using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNymphs : MonoBehaviour, IStunnable, ISlowable
{
    [Header("Movement")]
    public float speed = 2f;

    [Header("Settings")]
    public float damage = 10f;

    [Header("Status")]
    public bool isHooked = false;
    private bool isStunned = false;
    private bool isAttacking = false;
    private GameObject frogEgg;
    public Animator anim;
    private EggHealth targetEgg;

    void Start()
    {
        frogEgg = GameObject.FindGameObjectWithTag("FrogEgg");
    }

    void Update()
    {
        if (isHooked || isStunned || isAttacking) return;

        if (frogEgg != null)
            FindClosestEgg();

        FaceTarget();
    }
    void FaceTarget()
    {
        if (frogEgg == null) return;

        transform.position += transform.forward * speed * Time.deltaTime;

        // Direction from clone to target
        Vector3 direction = frogEgg.transform.position - transform.position;
        direction.y = 0f; // optional if you want to ignore vertical rotation

        if (direction != Vector3.zero)
        {
            // Instantly face the target
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
    void FindClosestEgg()
    {
        GameObject[] eggs = GameObject.FindGameObjectsWithTag("FrogEgg");
        GameObject closest = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject egg in eggs)
        {
            float dist = Vector3.Distance(transform.position, egg.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = egg;
            }
        }

        frogEgg = closest;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hook"))
        {
            DisapperWait();
            EnemyPool.Instance.ReturnToPool("Enemy4", gameObject);
        }
    }
    IEnumerator DisapperWait()
    {
        yield return new WaitForSeconds(1f);
        isHooked = true;
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("FrogEgg"))
        {
            if (!isAttacking)
            {
                isAttacking = true;
                anim.SetTrigger("NymphsAttack");
                targetEgg = collision.gameObject.GetComponent<EggHealth>();
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("FrogEgg"))
        {
            isAttacking = false;
            targetEgg = null;
        }
    }

    public void NymphsDamage()
    {
        if (targetEgg != null)
            targetEgg.TakeDamage(damage);
    }
    public void EndAttack()
    {
        isAttacking = false;
    }

    // -------------------- SLOW --------------------//
    public void SlowEffect(float slowMultiplier, float slowDuration)
    {
        float originalSpeed = speed;

        speed *= slowMultiplier;

        StartCoroutine(ResetSlow(originalSpeed, slowDuration));
    }

    private IEnumerator ResetSlow(float originalSpeed, float duration)
    {
        yield return new WaitForSeconds(duration);

        speed = originalSpeed;
    }

    // -------------------- STUN --------------------//
    public void Stun(float stunDuration)
    {
        StartCoroutine(StunCoroutine(stunDuration));
    }

    private IEnumerator StunCoroutine(float duration)
    {
        isStunned = true;

        float oldSpeed = speed;

        speed = 0;

        yield return new WaitForSeconds(duration);

        speed = oldSpeed;

        isStunned = false;
    }
}
