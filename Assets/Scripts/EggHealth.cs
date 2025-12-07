using UnityEngine;
using System.Collections;

public class EggHealth : MonoBehaviour
{
    [HideInInspector] public EggManager manager;

    public float maxHealth = 100f;
    public float damage = 10f;
    public float damageInterval = 0.5f;

    private float currentHealth;

    private bool isBeingDamaged = false;
    private Coroutine damageRoutine;

    private SpriteRenderer sprite;  // cached

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        // Fastest possible flash
        StartCoroutine(FlashEffect());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isBeingDamaged = false;

        if (damageRoutine != null)
            StopCoroutine(damageRoutine);

        manager.EggDied();

        gameObject.SetActive(false);
    }

    IEnumerator FlashEffect()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isBeingDamaged && other.CompareTag("PollutedWater"))
        {
            isBeingDamaged = true;
            damageRoutine = StartCoroutine(DamageLoop());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PollutedWater"))
        {
            isBeingDamaged = false;

            if (damageRoutine != null)
                StopCoroutine(damageRoutine);
        }
    }

    private IEnumerator DamageLoop()
    {
        while (isBeingDamaged)
        {
            TakeDamage(damage);
            yield return new WaitForSeconds(damageInterval);
        }
    }
}
