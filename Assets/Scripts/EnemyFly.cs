using System.Collections;
using UnityEngine;

public class EnemyFly : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 2f;
    public float zigzagFrequency = 3f;
    public float zigzagWidth = 1f;
    private float zigzagTimer;

    [Header("Status")]
    public bool isHooked = false;

    private Vector3 moveDirection;

    public void SetDirection(Vector3 dir)
    {
        moveDirection = dir;
    }
    void OnEnable()
    {
        // Reset state for pooling
        isHooked = false;

        zigzagTimer = 0f;
    }

    void Start()
    {
        zigzagTimer = 0f;
    }

    void Update()
    {
        if (isHooked) return;

        // Move forward
        Vector3 forwardMove = moveDirection * speed * Time.deltaTime;

        // Zigzag
        zigzagTimer += Time.deltaTime * zigzagFrequency;
        Vector3 right = Vector3.Cross(Vector3.up, moveDirection);
        Vector3 zigzagOffset = right * Mathf.Sin(zigzagTimer * Mathf.PI * 2) * zigzagWidth;

        transform.position += forwardMove + zigzagOffset * Time.deltaTime;

        // Face direction
        if (moveDirection == Vector3.left)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else
            transform.rotation = Quaternion.Euler(0, 180, 0);
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

    // -------------------- STATUS EFFECTS --------------------//

    // -------------------- SLOW --------------------//
    public void SlowEffect(float slowMultiplier, float slowDuration)
    {
        // store original values
        float originalSpeed = speed;
        float originalFrequency = zigzagFrequency;

        // apply slow
        speed *= slowMultiplier;              // slow forward speed
        zigzagFrequency *= slowMultiplier;    // slow zigzag motion

        StartCoroutine(ResetSlow(originalSpeed, originalFrequency, slowDuration));
    }

    private IEnumerator ResetSlow(float originalSpeed, float originalFrequency, float duration)
    {
        yield return new WaitForSeconds(duration);

        // reset values
        speed = originalSpeed;
        zigzagFrequency = originalFrequency;
    }

    // -------------------- STUN --------------------//
    // -------------------- STUN --------------------//
    public void Stun(float stunDuration)
    {
        StartCoroutine(StunCoroutine(stunDuration));
    }

    private IEnumerator StunCoroutine(float duration)
    {
        float oldSpeed = speed;
        float oldFrequency = zigzagFrequency;

        // stop movement
        speed = 0f;
        zigzagFrequency = 0f;

        yield return new WaitForSeconds(duration);

        // restore original values
        speed = oldSpeed;
        zigzagFrequency = oldFrequency;
    }

}
