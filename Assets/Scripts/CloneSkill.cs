using UnityEngine;

public class CloneSkill : MonoBehaviour
{
    [Header("Stats")]
    public float attackRange = 20f;
    public float attackCooldown = 1f;

    [Header("Enemy Tags")]
    public string[] enemyTags;

    private float attackTimer = 0f;
    private Transform targetEnemy;

    [Header("Hook Point")]
    public Transform tongueHook;

    private void OnEnable()
    {
        attackTimer = 0f;   // reset timer when clone spawns  
    }

    void Update()
    {
        FindNearestEnemy();
        AutoHook();
    }

    // ---------------- FIND ENEMY ----------------
    void FindNearestEnemy()
    {
        float closest = Mathf.Infinity;
        targetEnemy = null;

        foreach (string tag in enemyTags)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(tag);

            foreach (GameObject enemy in enemies)
            {
                float dist = Vector3.Distance(transform.position, enemy.transform.position);

                if (dist < attackRange && dist < closest)
                {
                    closest = dist;
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
            hook.SetTarget(tongueHook, targetEnemy);

            attackTimer = attackCooldown;
        }
    }
}
