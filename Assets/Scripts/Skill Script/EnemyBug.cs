using UnityEngine;
using System.Collections;

public class EnemyBug : MonoBehaviour, IStunnable, ISlowable
{
    public enum TargetType
    {
        Player,
        Egg
    }
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


    private void OnEnable()
    {
        isHooked = false;
        isSlow = false;
        isStunned = false;
        isAttacking = false;

        baseSpeed = speed;
        currentSpeed = baseSpeed;

        
        FindClosestEgg();

        ChooseRandomTarget();
    }



    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        frogEgg = GameObject.FindGameObjectWithTag("FrogEgg");
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
    void ChooseRandomTarget()
    {
        if (Random.value <= chanceToTargetEgg)
            currentTarget = TargetType.Egg;
        else
            currentTarget = TargetType.Player;
    }
    void HandleEggTarget()
    {
        FindClosestEgg();

        if (frogEgg == null)
        {
            DefaultMove();
            return;
        }

        float dist = Vector3.Distance(transform.position, frogEgg.transform.position);

        if (dist <= detectionRange)
        {
            ChaseEgg();
        }
        else
        {
            DefaultMove();
        }
    }
    void HandlePlayerTarget()
    {
        if (player == null)
        {
            DefaultMove();
            return;
        }

        Vector3 direction = (player.transform.position - transform.position).normalized;
        direction.y = 0f;

        if (direction != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(direction);

        transform.position += transform.forward * currentSpeed * 1.2f * Time.deltaTime;
    }
    void DefaultMove()
    {
        transform.position += Vector3.left * currentSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0f, -90f, 0f);
    }


    // -------------------- COLLISIONS -------------------- //
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hook"))
        {
            isHooked = true;
            EnemyPool.Instance.ReturnToPool("Enemy3", gameObject);
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (isAttacking) return;

        if (collision.gameObject.CompareTag("FrogEgg"))
        {
            isAttacking = true;
            anim.SetTrigger("GiantAttack");
            targetEgg = collision.gameObject.GetComponent<EggHealth>();
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            isAttacking = true;
            anim.SetTrigger("GiantAttack");
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

        transform.position += direction * currentSpeed * Time.deltaTime;
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
