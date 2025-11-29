using UnityEngine;

public class CloneSkill : MonoBehaviour
{
    [Header("Stats")]
    public float attackRange = 20f;
    public float hookCooldown = 1f;

    [Header("Upgrade Stats")]
    public int maxTargets = 1;
    public float consumeSpeed = 1f;
    public float consumeSpeedMultiplier = 1f;
    public float attackCooldown = 1f;
    public float cooldownModifier = 0f;

    [Header("Enemy Tags")]
    public string[] enemyTags;

    private Transform targetEnemy;
    private float attackTimer = 0f;
    private int consumedEnemies = 0;

    [Header("Hook Point")]
    public Transform tongueHook;

    private void OnEnable()
    {
        attackTimer = 0f;
        consumedEnemies = 0;
    }

    void Update()
    {
        if (consumedEnemies >= maxTargets)
        {
            HideClone();
            return;
        }

        FindNearestEnemy();
        FaceTarget();
        AutoHook();
    }

    void FaceTarget()
    {
        if (targetEnemy == null) return;

        // Direction from clone to target
        Vector3 direction = targetEnemy.position - transform.position;
        direction.y = 0f; // optional if you want to ignore vertical rotation

        if (direction != Vector3.zero)
        {
            // Instantly face the target
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    // ---------------- FIND NEAREST ENEMY ----------------
    void FindNearestEnemy()
    {
        float closestDist = Mathf.Infinity;
        targetEnemy = null;

        foreach (string tag in enemyTags)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(tag);

            foreach (GameObject enemy in enemies)
            {
                float dist = Vector3.Distance(transform.position, enemy.transform.position);
                if (dist < attackRange && dist < closestDist)
                {
                    closestDist = dist;
                    targetEnemy = enemy.transform;
                }
            }
        }
    }

    // ---------------- AUTO HOOK ----------------
    void AutoHook()
    {
        if (targetEnemy == null) return;

        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0f)
        {
            HookMechanism hook = HookPool.Instance.GetHook();

            //hook.tongueHook = tongueHook;
            hook.SetTarget(tongueHook, targetEnemy);

            // Apply consumption speed multiplier
            hook.hookSpeed *= consumeSpeedMultiplier;

            hook.onHookReturn = OnEnemyConsumed;

            attackTimer = hookCooldown + cooldownModifier;
        }
    }

    // ---------------- ON ENEMY CONSUMED ----------------
    void OnEnemyConsumed()
    {
        consumedEnemies++;
        if (consumedEnemies >= maxTargets)
        {
            HideClone();
        }
    }

    void HideClone()
    {
        gameObject.SetActive(false);
    }

    // ---------------- APPLY UPGRADE ----------------
    public void ApplyUpgrade(int newMaxTargets, float newConsumeSpeed, float newCooldownModifier)
    {
        maxTargets = newMaxTargets;
        consumeSpeedMultiplier = newConsumeSpeed;
        cooldownModifier = newCooldownModifier;

        // If your clone uses cooldown:
        attackCooldown += cooldownModifier;

        // If your clone uses hook consume time:
        consumeSpeed *= consumeSpeedMultiplier;
    }

}
