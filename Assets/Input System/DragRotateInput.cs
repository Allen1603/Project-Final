using UnityEngine;
using UnityEngine.InputSystem;

public class DragRotateInput : MonoBehaviour
{
    public float rotationSpeed = 0.3f;

    private Vector2 dragDelta;
    private bool dragging = false;

    // Called when the drag/touch input changes
    private Vector2 lastPos;

    public void OnRotate(InputAction.CallbackContext ctx)
    {
        Vector2 currentPos = ctx.ReadValue<Vector2>();

        if (dragging)
        {
            dragDelta = currentPos - lastPos; // calculate delta manually
        }

        lastPos = currentPos;
    }


    // Called when touch or mouse press/release
    public void OnPress(InputAction.CallbackContext ctx)
    {
        if (ctx.started) dragging = true;
        if (ctx.canceled)
        {
            dragging = false;
            dragDelta = Vector2.zero; // reset delta when released
        }
    }

    private void Update()
    {
        if (!dragging) return;

        // Rotate only around Y axis
        float rotY = -dragDelta.x * rotationSpeed * Time.deltaTime * 100f; // scaled for smoothness
        transform.Rotate(0f, rotY, 0f, Space.World);
    }
}
