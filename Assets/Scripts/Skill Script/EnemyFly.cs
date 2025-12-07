using System.Collections;
using UnityEngine;

public class EnemyFly : MonoBehaviour, IStunnable, ISlowable
{
    [Header("Movement")]
    private float currentSpeed;
    public float speed = 2f;
    public float zigzagFrequency = 3f;
    public float zigzagWidth = 1f;
    private float zigzagTimer;
    private bool isStunned = false;

    [Header("Laying Egg")]
    public float layingEggTimer = 2f;
    private float currentTimer;
    public GameObject flyEggPrefab;
    public Transform eggPosition;
    private bool isLayingEgg = false;

    [Header("Status")]
    public bool isHooked = false;

    public Animator anim;

    void OnEnable()
    {
        isHooked = false;
        zigzagTimer = 0f;
    }
    private void Start()
    {
        currentTimer = layingEggTimer;
        currentSpeed = speed;
    }

    void Update()
    {
        if (isHooked || isStunned || isLayingEgg) return;

        // ---- ALWAYS MOVE LEFT ---- //
        Vector3 forwardMove = Vector3.left * currentSpeed * Time.deltaTime;

        // ---- ZIGZAG ---- //
        zigzagTimer += Time.deltaTime * zigzagFrequency;
        float zigzagOffset = Mathf.Sin(zigzagTimer * Mathf.PI * 2) * zigzagWidth;

        Vector3 zigzagMove = new Vector3(0f, 0f, zigzagOffset * Time.deltaTime);

        transform.position += forwardMove + zigzagMove;

        LayingEgg();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hook"))
        {
            isHooked = true;
        }
        if (other.CompareTag("Player"))
        {
            PlayerController.instance.TakeBar(10f);
            PlayerController.instance.TakeExp(10f);
            EnemyPool.Instance.ReturnToPool("Enemy2", gameObject);
        }
        if (other.CompareTag("Base"))
        {
            EnemyPool.Instance.ReturnToPool("Enemy2", gameObject);
        }
    }

    void LayingEgg()
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
        isLayingEgg = false;   // ⬅ RESUME movement
    }

    // -------------------- SLOW --------------------//
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

    // -------------------- STUN --------------------//
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
