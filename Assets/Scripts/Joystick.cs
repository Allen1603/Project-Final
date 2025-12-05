using UnityEngine;
using UnityEngine.InputSystem;

public class Joystick : MonoBehaviour
{
    [Header("-------Input------")]
    private PlayerInput playerInput;
    private InputAction moveAction;

    [Header("------Hook Settings------")]
    public Transform tongueHook;
    public int hookPrefabIndex = 3;
    private bool isFishing = false;

    [Header("-----Settings----")]
    public float rotationSpeed = 10f;

    [Header("-------Animation----")]
    public Animator anim;

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
        anim.SetTrigger("FrogAttack");

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("froggy");
        } 
    }

    private void HookReturned()
    {
        isFishing = false;
    }

    public void AttackEvent()
    {
        HookMechanism hook = HookPool.Instance.GetHook(hookPrefabIndex);

        // Setup hook to use tongueHook as origin
        hook.SetUpHook(tongueHook);

        isFishing = true;
        hook.onHookReturn = HookReturned;
    }
}
