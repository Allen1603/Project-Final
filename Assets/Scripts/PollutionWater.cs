using System.Collections;
using UnityEngine;

public class PollutionWater : MonoBehaviour
{
    public float damage = 10f;
    public float damageInterval = 0.5f;
    public float waterSpeed = 2f;
    public float zigzagFrequency = 2f;
    public float zigzagWidth = 1f;
    private float zigzagTimer;

    private void Start()
    {
        //transform.position = new Vector3(7.16f, 0.55f, 5.43f);

    }
    private void Update()
    {
        // ---- ALWAYS MOVE LEFT ---- //
        Vector3 forwardMove = Vector3.left * waterSpeed * Time.deltaTime;

        // ---- ZIGZAG ---- //
        zigzagTimer += Time.deltaTime * zigzagFrequency;
        float zigzagOffset = Mathf.Sin(zigzagTimer * Mathf.PI * 2) * zigzagWidth;

        Vector3 zigzagMove = new Vector3(0f, 0f, zigzagOffset * Time.deltaTime);

        transform.position += forwardMove + zigzagMove;
    }
    private void OnTriggerStay(Collider other)
    {
        EggHealth egg = other.GetComponent<EggHealth>();

        if (egg != null)
        {
            StartCoroutine(DamageEgg(egg));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        EggHealth egg = other.GetComponent<EggHealth>();
        if (egg != null)
        {
            egg.isBeingDamaged = false;   // Stop damaging THIS egg
        }
    }

    private IEnumerator DamageEgg(EggHealth egg)
    {
        if (egg.isBeingDamaged) yield break;

        egg.isBeingDamaged = true;

        while (egg != null && egg.gameObject.activeSelf && egg.isBeingDamaged)
        {
            egg.TakeDamage(damage);
            yield return new WaitForSeconds(damageInterval);
        }

        egg.isBeingDamaged = false;
    }
}
