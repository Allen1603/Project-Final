using UnityEngine;
using UnityEngine.EventSystems;

public class DragRotate : MonoBehaviour
{
    [Tooltip("How fast the model rotates.")]
    public float rotationSpeed = 0.3f;

    Camera cam;
    bool dragging = false;
    Vector3 lastPos;

    void Start()
    {
        cam = Camera.main;
        if (cam == null) Debug.LogWarning("[DragRotateRobust] No Camera.main found.");
        // Make sure this object (or a child) has a collider so raycasts hit it
        if (GetComponentInChildren<Collider>() == null)
            Debug.LogWarning("[DragRotateRobust] No collider found on this object or its children. Add a Collider (Box, MeshCollider, etc.)");
    }

    void Update()
    {
        // --- MOUSE ---
        if (Input.GetMouseButtonDown(0))
        {
            // ignore if pointer is over UI
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;

            if (TryHitThisObject(Input.mousePosition))
            {
                dragging = true;
                lastPos = Input.mousePosition;
            }
        }

        if (Input.GetMouseButtonUp(0))
            dragging = false;

        if (dragging && Input.GetMouseButton(0))
            DoRotate(Input.mousePosition);

        // --- TOUCH ---
        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);

            if (t.phase == TouchPhase.Began)
            {
                // ignore UI touches
                if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(t.fingerId)) return;

                if (TryHitThisObject(t.position))
                {
                    dragging = true;
                    lastPos = t.position;
                }
            }
            else if (t.phase == TouchPhase.Moved && dragging)
            {
                DoRotate(t.position);
            }
            else if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled)
            {
                dragging = false;
            }
        }
    }

    bool TryHitThisObject(Vector2 screenPos)
    {
        if (cam == null) cam = Camera.main;
        Ray r = cam.ScreenPointToRay(screenPos);
        if (Physics.Raycast(r, out RaycastHit hit, 100f))
        {
            // hit this object or its children?
            if (hit.transform == this.transform || hit.transform.IsChildOf(this.transform))
            {
                return true;
            }
        }
        return false;
    }

    void DoRotate(Vector3 currentPos)
    {
        Vector3 delta = currentPos - lastPos;

        // Rotate around camera axes so drag direction is intuitive regardless of camera orientation
        Vector3 camUp = (cam != null) ? cam.transform.up : Vector3.up;
        Vector3 camRight = (cam != null) ? cam.transform.right : Vector3.right;

        // Horizontal drag -> rotate around camUp (Y-like)
        transform.Rotate(camUp, -delta.x * rotationSpeed, Space.World);

        // Vertical drag -> rotate around camRight (tilt)
        transform.Rotate(camRight, delta.y * rotationSpeed, Space.World);

        lastPos = currentPos;
    }
}
