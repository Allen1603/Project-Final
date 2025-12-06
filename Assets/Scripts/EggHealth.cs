using UnityEngine;
using System;
using System.Collections;

public class EggHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    public bool isBeingDamaged = false;

    private void Start()
    {
        currentHealth = maxHealth;
        this.gameObject.SetActive(true);
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        StartCoroutine(DamageFlash());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isBeingDamaged = false;
        this.gameObject.SetActive(false);
    }
    IEnumerator DamageFlash()
    {
        Renderer r = GetComponent<Renderer>();
        r.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        r.material.color = Color.white;
    }
}
