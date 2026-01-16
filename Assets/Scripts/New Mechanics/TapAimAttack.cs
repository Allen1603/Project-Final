using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class TapAimAttack : MonoBehaviour
{
    [Header("----- Hook -----")]
    public Transform tongueOrigin;
    public int hookPrefabIndex = 3;

    [Header("----- Aiming -----")]
    public LayerMask groundLayer;
    public float rotationSpeed = 15f;
    public GameObject aimAttack;

    [Header("----- Cooldown -----")]
    public float attackCooldown = 0.4f;

    [Header("----- Animation -----")]
    public Animator anim;

    private Camera cam;
    private bool isAiming;
    private bool hasFired;
    private float lastAttackTime;
    private Vector3 aimWorldPoint;

    // Touch
    private int activeTouchId = -1;

    private void Awake()
    {
        cam = Camera.main;
        if (anim == null)
            anim = GetComponent<Animator>();

        if (aimAttack != null)
            aimAttack.SetActive(false);
    }

    private void Update()
    {
        HandleTouch();
        HandleMouse();

        if (isAiming)
        {
            RotateTowards(aimWorldPoint);

            if (aimAttack != null && !aimAttack.activeSelf)
                aimAttack.SetActive(true);
        }
        else
        {
            if (aimAttack != null && aimAttack.activeSelf)
                aimAttack.SetActive(false);
        }
    }

    // ================= TOUCH INPUT =================

    private void HandleTouch()
    {
        if (Touchscreen.current == null) return;

        foreach (var touch in Touchscreen.current.touches)
        {
            int id = touch.touchId.ReadValue();

            // TOUCH START
            if (touch.press.wasPressedThisFrame)
            {
                if (EventSystem.current != null &&
                    EventSystem.current.IsPointerOverGameObject(id))
                    continue;

                activeTouchId = id;
                isAiming = true;
                hasFired = false;
            }

            // TOUCH HOLD
            if (isAiming && id == activeTouchId && touch.press.isPressed)
            {
                UpdateAimPoint(touch.position.ReadValue());
            }

            // TOUCH RELEASE
            if (isAiming && id == activeTouchId &&
                touch.press.wasReleasedThisFrame && !hasFired)
            {
                hasFired = true;
                FireAttack();

                activeTouchId = -1;
                isAiming = false;
                return;
            }
        }
    }

    // ================= MOUSE INPUT =================

    private void HandleMouse()
    {
        if (Mouse.current == null) return;

        // MOUSE DOWN
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (EventSystem.current != null &&
                EventSystem.current.IsPointerOverGameObject())
                return;

            isAiming = true;
            hasFired = false;
        }

        // MOUSE HOLD
        if (isAiming && Mouse.current.leftButton.isPressed)
        {
            UpdateAimPoint(Mouse.current.position.ReadValue());
        }

        // MOUSE RELEASE
        if (isAiming && Mouse.current.leftButton.wasReleasedThisFrame && !hasFired)
        {
            hasFired = true;
            FireAttack();
            isAiming = false;
        }
    }

    // ================= AIM =================

    private void UpdateAimPoint(Vector2 screenPos)
    {
        Ray ray = cam.ScreenPointToRay(screenPos);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer))
        {
            aimWorldPoint = hit.point;
        }
    }

    private void RotateTowards(Vector3 target)
    {
        Vector3 dir = target - transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.01f) return;

        Quaternion targetRot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRot,
            rotationSpeed * Time.deltaTime
        );
    }

    // ================= ATTACK =================

    private void FireAttack()
    {
        if (Time.time - lastAttackTime < attackCooldown) return;
        lastAttackTime = Time.time;

        if (anim != null)
            anim.SetTrigger("FrogAttack");

        HookMechanism hook = HookPool.Instance.GetHook(hookPrefabIndex);
        hook.SetUpHook(tongueOrigin);
    }
}
