using UnityEngine;
using System.Collections;

public class EnemyWaterbug : EnemyBase, IStunnable, ISlowable
{
    public enum TargetType { Player, Egg }

    [Header("Movement")]
    public float speed = 2f;
    public float detectionRange = 5f;

    [Header("Combat")]
    public float damage = 20f;
    public Animator anim;

    [Header("Targeting")]
    public TargetType currentTarget;
    [Range(0f, 1f)]
    public float chanceToTargetEgg = 0.5f;

    [Header("Status")]
    public bool isHooked = false;

    private float baseSpeed;
    private float currentSpeed;
    private bool isSlow;
    private bool isStunned;
    private bool isAttacking;

    private GameObject player;
    private GameObject frogEgg;
    private EggHealth targetEgg;

    protected override void OnEnable()
    {
        base.OnEnable();
        // Reset state
        isHooked = false;
        isSlow = false;
        isStunned = false;
        isAttacking = false;

        baseSpeed = speed;
        currentSpeed = baseSpeed;

        player = GameObject.FindGameObjectWithTag("Player");
        FindClosestEgg();
        ChooseRandomTarget();
    }

    private void Update()
    {
        if (isHooked || isStunned || isAttacking) return;

        switch (currentTarget)
        {
            case TargetType.Egg:
                HandleEggTarget();
                break;
            case TargetType.Player:
                HandlePlayerTarget();
                break;
        }
    }

    private void ChooseRandomTarget()
    {
        currentTarget = (Random.value <= chanceToTargetEgg) ? TargetType.Egg : TargetType.Player;
    }

    private void HandleEggTarget()
    {
        // Find closest egg every frame (flat XZ distance)
        FindClosestEgg();

        if (frogEgg == null)
        {
            DefaultMove();
            return;
        }

        // Check distance on XZ plane
        Vector3 flatEnemyPos = new Vector3(transform.position.x, 0f, transform.position.z);
        Vector3 flatEggPos = new Vector3(frogEgg.transform.position.x, 0f, frogEgg.transform.position.z);
        float dist = Vector3.Distance(flatEnemyPos, flatEggPos);

        if (dist <= detectionRange)
            ChaseEgg();
        else
            DefaultMove();
    }

    private void HandlePlayerTarget()
    {
        if (player == null)
        {
            DefaultMove();
            return;
        }

        Vector3 direction = player.transform.position - transform.position;
        direction.y = 0f;
        if (direction != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 8f * Time.deltaTime);
        }

        transform.position += transform.forward * currentSpeed * 1.2f * Time.deltaTime;
    }

    private void DefaultMove()
    {
        transform.position += Vector3.left * currentSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0f, -90f, 0f);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (isAttacking) return;

        if (collision.gameObject.CompareTag("FrogEgg"))
        {
            isAttacking = true;
            anim.SetTrigger("BugAttack");
            targetEgg = collision.gameObject.GetComponent<EggHealth>();
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            isAttacking = true;
            anim.SetTrigger("BugAttack");
            PlayerController.instance.TakeDamage(10f);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("FrogEgg") || collision.gameObject.CompareTag("Player"))
        {
            isAttacking = false;
            targetEgg = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hook"))
        {
            ResetEnemy();
            EnemyPool.Instance.ReturnToPool("Enemy1", gameObject);
        }
    }

    void FindClosestEgg()
    {
        GameObject[] eggs = GameObject.FindGameObjectsWithTag("FrogEgg");
        if (eggs.Length == 0)
        {
            frogEgg = null;
            return;
        }

        GameObject closest = null;
        float minDist = Mathf.Infinity;

        Vector3 flatEnemyPos = new Vector3(transform.position.x, 0f, transform.position.z);

        foreach (GameObject egg in eggs)
        {
            Vector3 flatEggPos = new Vector3(egg.transform.position.x, 0f, egg.transform.position.z);
            float dist = Vector3.Distance(flatEnemyPos, flatEggPos);

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
        direction.y = 0f; // ignore vertical distance

        if (direction != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 6f * Time.deltaTime);
        }

        transform.position += transform.forward * currentSpeed * Time.deltaTime;
    }


    public void BugDamage()
    {
        targetEgg?.TakeDamage(damage);
    }

    public void EndAttack()
    {
        isAttacking = false;
    }

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

    public void Stun(float duration)
    {
        StartCoroutine(StunCoroutine(duration));
    }

    private IEnumerator StunCoroutine(float duration)
    {
        isStunned = true;
        float oldSpeed = currentSpeed;
        currentSpeed = 0f;

        yield return new WaitForSeconds(duration);

        isStunned = false;
        currentSpeed = isSlow ? baseSpeed * 0.5f : baseSpeed;
    }

    private void ResetEnemy()
    {
        // Reset all states before returning to pool
        isHooked = false;
        isSlow = false;
        isStunned = false;
        isAttacking = false;
        currentSpeed = baseSpeed;
        targetEgg = null;
    }
}
