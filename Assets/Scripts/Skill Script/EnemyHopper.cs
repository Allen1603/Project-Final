using System.Collections;
using UnityEngine;

public class EnemyHopper : MonoBehaviour, IStunnable, ISlowable
{
    [Header("Movement")]
    public float speed = 2f;
    public float jumpHeight = 1f;
    public float jumpDuration = 0.5f;
    public float jumpInterval = 3f;

    private float jumpTimer;
    private bool isJumping = false;
    private float jumpStartTime;
    private Vector3 jumpStartPos;
    public bool isHooked = false;


    void Start()
    {
        jumpTimer = jumpInterval;
    }

    void Update()
    {
        if (isHooked) return;

        // ---- MOVE LEFT ONLY ----
        transform.position += Vector3.left * speed * Time.deltaTime;

        // ---- JUMPING ----
        jumpTimer -= Time.deltaTime;
        if (!isJumping && jumpTimer <= 0f)
        {
            isJumping = true;
            jumpStartTime = Time.time;
            jumpStartPos = transform.position;
            jumpTimer = jumpInterval;
        }

        if (isJumping)
        {
            float elapsed = Time.time - jumpStartTime;
            float percent = elapsed / jumpDuration;

            if (percent >= 1f)
            {
                isJumping = false;
                transform.position = new Vector3(transform.position.x, jumpStartPos.y, transform.position.z);
            }
            else
            {
                float yOffset = Mathf.Sin(percent * Mathf.PI) * jumpHeight;
                transform.position = new Vector3(transform.position.x, jumpStartPos.y + yOffset, transform.position.z);
            }
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hook"))
        {
            isHooked = true;
        }
        if (other.CompareTag("Player"))
        {
            PlayerController.instance.TakeBar(10f);
            PlayerController.instance.TakeExp(10f);
            EnemyPool.Instance.ReturnToPool("Enemy1", gameObject);
        }
        if (other.CompareTag("Base"))
        {
            EnemyPool.Instance.ReturnToPool("Enemy1", gameObject);
        }
    }

    // -------------------- STATUS EFFECTS --------------------//

    // -------------------- SLOW --------------------//
    public void SlowEffect(float slowMultiplier, float slowDuration)
    {
        // Save original values
        float originalSpeed = speed;
        float originalJumpDuration = jumpDuration;
        float originalJumpInterval = jumpInterval;

        // Apply slow
        speed *= slowMultiplier;              // slow forward movement
        jumpDuration /= slowMultiplier;       // longer jump animation (slower jump)
        jumpInterval /= slowMultiplier;       // longer wait between jumps

        StartCoroutine(ResetSlow(originalSpeed, originalJumpDuration, originalJumpInterval, slowDuration));
    }

    private IEnumerator ResetSlow(float oSpeed, float oJumpDuration, float oJumpInterval, float duration)
    {
        yield return new WaitForSeconds(duration);

        // Reset to original
        speed = oSpeed;
        jumpDuration = oJumpDuration;
        jumpInterval = oJumpInterval;
    }

    // -------------------- STUN --------------------//
    public void Stun(float stunDuration)
    {
        StartCoroutine(StunCoroutine(stunDuration));
    }

    private IEnumerator StunCoroutine(float duration)
    {
        float oldSpeed = speed;
        float oldJumpDuration = jumpDuration;
        float oldJumpInterval = jumpInterval;

        // STOP ALL movement and jumping
        speed = 0f;
        jumpDuration = 9999f;   // basically freezing jump animation
        jumpInterval = 9999f;   // no more jumps

        isJumping = false; // stop current jump immediately

        yield return new WaitForSeconds(duration);

        // Restore
        speed = oldSpeed;
        jumpDuration = oldJumpDuration;
        jumpInterval = oldJumpInterval;
    }
}
