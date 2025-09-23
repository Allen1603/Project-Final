using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Joystick : MonoBehaviour
{
    [Header("Input")]
    private PlayerInput playerInput;
    private InputAction moveAction;

    [Header("Hook Settings")]
    private HookMechanism hook;
    public GameObject hookPrefab;
    private Transform cachedTransform;
    private bool isFishing = false;

    [Header("Settings")]
    public float rotationSpeed = 10f; // smooth turning speed

    private void Awake()
    {
        // Get PlayerInput
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["attack"]; // <-- use Move (Vector2), not attack

        cachedTransform = transform;
    }

    private void OnEnable()
    {
        // Subscribe to joystick release event
        moveAction.canceled += OnJoystickReleased;
    }

    private void OnDisable()
    {
        // Unsubscribe when disabled
        moveAction.canceled -= OnJoystickReleased;
    }

    private void Update()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();

        if (input.sqrMagnitude > 0.01f) // joystick is being moved
        {
            // Convert joystick input into a 3D direction (XZ plane)
            Vector3 direction = new Vector3(input.x, 0, input.y);

            // Get target rotation
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

            // Smooth rotate character
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void OnJoystickReleased(InputAction.CallbackContext context)
    {
        if (!isFishing) // only attack if not already casting
        {
            LaunchHook();
        }
    }

    private void LaunchHook()
    {
        var hookObject = Instantiate(hookPrefab, cachedTransform.position, Quaternion.identity);
        hook = hookObject.GetComponent<HookMechanism>();
        hook.SetUpHook(cachedTransform);
        isFishing = true;

        // Callback when hook returns
        hook.onHookReturn = HookReturned;
    }

    public void HookReturned()
    {
        isFishing = false;
    }
}
