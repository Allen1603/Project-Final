using UnityEngine;
using System.Collections;

public class EnemyBug : MonoBehaviour, IStunnable, ISlowable
{
    [Header("Movement Settings")]
    public float speed = 2f;
    private float baseSpeed;
    private float currentSpeed;
    public float detectionRange = 5f;

    [Header("Status")]
    public bool isHooked = false;
    private bool isSlow = false;
    private bool isStunned = false;
    private bool isAttacking = false;

    private GameObject frogEgg;
    private EggHealth targetEgg;
    public float damage = 20f;
    public Animator anim;

    private void OnEnable()
    {
        isHooked = false;
        isSlow = false;
        isStunned = false;

        baseSpeed = speed;
        currentSpeed = baseSpeed;
    }

    void Start()
    {
        frogEgg = GameObject.FindGameObjectWithTag("FrogEgg");
    }

    private void Update()
    {
        if (isHooked || isStunned || isAttacking) return;

        FindClosestEgg();

        if (frogEgg != null)
        {
            float dist = Vector3.Distance(transform.position, frogEgg.transform.position);

            if (dist <= detectionRange)
            {
                // Chase egg
                ChaseEgg();
                return;
            }
        }
        transform.position += Vector3.left * currentSpeed * Time.deltaTime;
    }
    // -------------------- COLLISIONS -------------------- //
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hook"))
            isHooked = true;

        if (other.CompareTag("Base"))
            EnemyPool.Instance.ReturnToPool("Enemy3", gameObject);
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("FrogEgg"))
        {
            if (!isAttacking)
            {
                isAttacking = true;
                anim.SetTrigger("GiantAttack");
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
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController.instance.TakeBar(10f);
            PlayerController.instance.TakeExp(10f);
            EnemyPool.Instance.ReturnToPool("Enemy3", gameObject);
        }
    }
    // -------------------- FIND CLOSEST EGG --------------------
    void FindClosestEgg()
    {
        GameObject[] eggs = GameObject.FindGameObjectsWithTag("FrogEgg");
        if (eggs.Length == 0) return;

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

    // -------------------- CHASE + ROTATE --------------------
    void ChaseEgg()
    {
        if (frogEgg == null) return;

        Vector3 direction = frogEgg.transform.position - transform.position;
        direction.y = 0f;

        if (direction != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                6f * Time.deltaTime // smooth turning
            );
        }

        transform.position += transform.forward * currentSpeed * Time.deltaTime;
    }
    public void GiantBugDamage()
    {
        if (targetEgg != null)
            targetEgg.TakeDamage(damage);
    }
    public void EndAttack()
    {
        isAttacking = false;
    }
    // -------------------- SLOW -------------------- //
    public void SlowEffect(float slowSpeed, float duration)
    {
        if (isSlow) return;

        isSlow = true;
        currentSpeed = slowSpeed;

        StartCoroutine(ResetSlow(duration));
    }

    private IEnumerator ResetSlow(float duration)
    {
        yield return new WaitForSeconds(duration);
        isSlow = false;
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

        float oldSpeed = currentSpeed;
        currentSpeed = 0;

        yield return new WaitForSeconds(duration);

        isStunned = false;

        currentSpeed = isSlow ? baseSpeed * 0.5f : baseSpeed;
    }
}
