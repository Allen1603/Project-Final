using UnityEngine;
using UnityEngine.InputSystem;

public class Joystick : MonoBehaviour
{
    [Header("-------Input------")]
    private PlayerInput playerInput;
    private InputAction moveAction;

    [Header("------Hook Settings------")]
    public Transform tongueHook;
    private bool isFishing = false;

    [Header("-----Settings----")]
    public float rotationSpeed = 10f;

    [Header("-------Animation----")]
    public Animator anim;

    [Header("-----SFX Settings-----")]
    public float attackSFXCooldown = 0.25f;
    private float lastSFXTime = -10f;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["attack"];

        if (anim == null)
            anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        moveAction.canceled += OnJoystickReleased;
    }

    private void OnDisable()
    {
        moveAction.canceled -= OnJoystickReleased;
    }

    private void Update()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();

        if (input.sqrMagnitude > 0.01f)
        {
            // Only rotate and animate — no SFX here (to avoid spam)
            anim.SetTrigger("Attack1");

            Vector3 direction = new Vector3(input.x, 0, input.y);
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void OnJoystickReleased(InputAction.CallbackContext context)
    {
        if (!isFishing)
        {
            LaunchHook();
        }
    }

    private void LaunchHook()
    {
        anim.SetTrigger("Attack");

        // Play SFX only on release + cooldown to prevent spam
        if (Time.time - lastSFXTime >= attackSFXCooldown)
        {
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlaySFX("froggy");

            lastSFXTime = Time.time;
        }

        HookMechanism hook = HookPool.Instance.GetHook();
        hook.SetUpHook(tongueHook);

        isFishing = true;
        hook.onHookReturn = HookReturned;
    }

    private void HookReturned()
    {
        isFishing = false;
    }
}
