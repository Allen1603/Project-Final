using System.Collections;
using UnityEngine;

public class EnemyNymphs : EnemyBase, IStunnable, ISlowable
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
    private GameObject player;
    private EggHealth targetEgg;
    public Animator anim;

    public enum TargetType { Player, Egg }

    [Header("Targeting")]
    public TargetType currentTarget;
    [Range(0f, 1f)]
    public float chanceToTargetEgg = 0.6f;

    private float baseSpeed;

    // -------------------- UNITY EVENTS -------------------- //
    protected override void OnEnable()
    {
        base.OnEnable();
        baseSpeed = speed;
        ResetStatus();

        player = GameObject.FindGameObjectWithTag("Player");
        FindClosestEgg();

        ChooseRandomTarget();
        FaceCurrentTarget();
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hook"))
        {
            ResetStatus();
            EnemyPool.Instance.ReturnToPool("Enemy4", gameObject);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (isAttacking) return;

        if (collision.gameObject.CompareTag("FrogEgg"))
            StartAttack(collision.gameObject.GetComponent<EggHealth>());
        else if (collision.gameObject.CompareTag("Player"))
            StartAttackOnPlayer();
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("FrogEgg") || collision.gameObject.CompareTag("Player"))
            EndAttack();
    }

    // -------------------- MOVEMENT & TARGETING -------------------- //
    private void HandleEggTarget()
    {
        if (frogEgg == null) FindClosestEgg();
        if (frogEgg == null) { DefaultMove(); return; }

        MoveTowards(frogEgg.transform.position);
    }

    private void HandlePlayerTarget()
    {
        if (player == null) { DefaultMove(); return; }

        MoveTowards(player.transform.position);
    }

    private void MoveTowards(Vector3 targetPos)
    {
        Vector3 direction = (targetPos - transform.position).normalized;
        direction.y = 0f;

        if (direction != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(direction);

        transform.position += direction * speed * Time.deltaTime;
    }

    private void DefaultMove()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0f, -90f, 0f);
    }

    private void FaceCurrentTarget()
    {
        Vector3 targetPos = currentTarget == TargetType.Player ? player?.transform.position ?? transform.position
                                                               : frogEgg?.transform.position ?? transform.position;

        Vector3 direction = (targetPos - transform.position).normalized;
        direction.y = 0f;

        if (direction != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(direction);
    }

    private void ChooseRandomTarget()
    {
        currentTarget = Random.value <= chanceToTargetEgg ? TargetType.Egg : TargetType.Player;
    }

    private void FindClosestEgg()
    {
        GameObject[] eggs = GameObject.FindGameObjectsWithTag("FrogEgg");
        if (eggs.Length == 0) { frogEgg = null; return; }

        float minDist = Mathf.Infinity;
        GameObject closest = null;

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

    // -------------------- ATTACK -------------------- //
    private void StartAttack(EggHealth egg)
    {
        isAttacking = true;
        anim.SetTrigger("NymphsAttack");
        targetEgg = egg;
    }

    private void StartAttackOnPlayer()
    {
        isAttacking = true;
        anim.SetTrigger("NymphsAttack");
        PlayerController.instance.TakeDamage(damage);
    }

    public void NymphsDamage()
    {
        targetEgg?.TakeDamage(damage);
    }

    public void EndAttack()
    {
        isAttacking = false;
        targetEgg = null;
    }

    // -------------------- STATUS -------------------- //
    private void ResetStatus()
    {
        isHooked = false;
        isStunned = false;
        isAttacking = false;
        targetEgg = null;
        // don't reset speed here
    }

    public void SlowEffect(float slowMultiplier, float slowDuration)
    {
        speed = baseSpeed * slowMultiplier;
        StartCoroutine(ResetSlow(slowDuration));
    }

    private IEnumerator ResetSlow(float duration)
    {
        yield return new WaitForSeconds(duration);
        speed = baseSpeed;
    }

    public void Stun(float stunDuration)
    {
        StartCoroutine(StunCoroutine(stunDuration));
    }

    private IEnumerator StunCoroutine(float duration)
    {
        isStunned = true;
        float oldSpeed = speed;
        speed = 0f;

        yield return new WaitForSeconds(duration);

        speed = oldSpeed;
        isStunned = false;
    }
}
