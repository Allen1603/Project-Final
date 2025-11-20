using UnityEngine;
using UnityEngine.InputSystem;

public class Joystick : MonoBehaviour
{
    [Header("-------Input------")]
    private PlayerInput playerInput;
    private InputAction moveAction;

    [Header("------Hook Settings------")]
    public GameObject hookPrefab;
    public Transform tongueHook;
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
            // Joystick is being held
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
    //-----ADD EVENT----
    //public void AnimationLaunchHook()
    //{
    //    LaunchHook();
    //}
    private void LaunchHook()
    {
        anim.SetTrigger("Attack");

        HookMechanism hook = HookPool.Instance.GetHook();

        // Setup hook to use tongueHook as origin
        hook.SetUpHook(tongueHook); 

        isFishing = true;
        hook.onHookReturn = HookReturned;
    }

    private void HookReturned()
    {
        isFishing = false;
    }
}
