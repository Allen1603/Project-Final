using System.Collections;
using UnityEngine;

public class EnemyFly : EnemyBase, IStunnable, ISlowable
{
    [Header("Movement")]
    public float speed = 2f;
    public float zigzagFrequency = 3f;
    public float zigzagWidth = 1f;
    private float zigzagTimer;
    private bool isStunned = false;

    [Header("Laying Egg")]
    public float layingEggTimer = 2f;
    private float currentTimer;
    public Transform eggPosition;
    private bool isLayingEgg = false;

    [Header("Status")]
    public bool isHooked = false;

    public Animator anim;
    public Transform[] rondaPos;
    private Transform targetPoint;
    public float rotateSpeed = 5f;

    // -------------------- UNITY EVENTS -------------------- //
    protected override void OnEnable()
    {
        base.OnEnable(); // call base class OnEnable if any

        isHooked = false;
        zigzagTimer = 0f;
        currentTimer = layingEggTimer;
        isLayingEgg = false;

        PickNewRondaPoint();
    }

    private void Update()
    {
        if (isHooked || isStunned || isLayingEgg) return;

        MoveFly();
        ZigzagMovement();
        LayingEgg();
    }

    // -------------------- MOVEMENT -------------------- //
    private void MoveFly()
    {
        if (targetPoint == null) return;

        Vector3 direction = (targetPoint.position - transform.position).normalized;
        direction.y = 0f;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPoint.position) < 0.2f)
        {
            PickNewRondaPoint();
        }
    }

    private void ZigzagMovement()
    {
        zigzagTimer += Time.deltaTime * zigzagFrequency;
        float zigzagOffset = Mathf.Sin(zigzagTimer * Mathf.PI * 2) * zigzagWidth;
        transform.position += new Vector3(0f, 0f, zigzagOffset * Time.deltaTime);
    }

    private void PickNewRondaPoint()
    {
        if (rondaPos.Length == 0) return;
        targetPoint = rondaPos[Random.Range(0, rondaPos.Length)];
    }

    // -------------------- EGG LAYING -------------------- //
    private void LayingEgg()
    {
        currentTimer -= Time.deltaTime;

        if (currentTimer <= 0f)
        {
            isLayingEgg = true;
            anim.SetTrigger("LayingEgg");
            currentTimer = layingEggTimer;
        }
    }

    public void DeployingEgg()
    {
        EnemyPool.Instance.SpawnFromPool("Egg", eggPosition.position, Quaternion.identity);
    }

    public void FinishLayingEgg()
    {
        isLayingEgg = false;
    }

    // -------------------- TRIGGERS -------------------- //
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hook"))
        {
            isHooked = true;
            EnemyPool.Instance.ReturnToPool("Enemy2", gameObject);
        }
    }

    // -------------------- SLOW -------------------- //
    public void SlowEffect(float slowMultiplier, float slowDuration)
    {
        float originalSpeed = speed;
        float originalFrequency = zigzagFrequency;

        speed *= slowMultiplier;
        zigzagFrequency *= slowMultiplier;

        StartCoroutine(ResetSlow(originalSpeed, originalFrequency, slowDuration));
    }

    private IEnumerator ResetSlow(float originalSpeed, float originalFrequency, float duration)
    {
        yield return new WaitForSeconds(duration);

        speed = originalSpeed;
        zigzagFrequency = originalFrequency;
    }

    // -------------------- STUN -------------------- //
    public void Stun(float stunDuration)
    {
        StartCoroutine(StunCoroutine(stunDuration));
    }

    private IEnumerator StunCoroutine(float duration)
    {
        isStunned = true;

        float oldSpeed = speed;
        float oldFreq = zigzagFrequency;

        speed = 0;
        zigzagFrequency = 0;

        yield return new WaitForSeconds(duration);

        speed = oldSpeed;
        zigzagFrequency = oldFreq;

        isStunned = false;
    }
}
