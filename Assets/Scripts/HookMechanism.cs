using UnityEngine;

public class HookMechanism : MonoBehaviour
{
    private Vector3 startPoint;
    private Vector3 targetPoint;
    private float hookProgress = 0f;

    private GameObject hookedEnemy;
    private LineRenderer lineRenderer;
    private Collider hookCollider;   // we'll manage this
    public Transform tongueHook;

    [Header("Hook Settings")]
    public float hookRange = 5f;
    public float hookSpeed = 20f;
    public float returnSpeed = 25f;

    // States
    public bool IsReturning { get; private set; }
    public bool IsMovingForward { get; private set; }
    public bool HasCaughtEnemy { get; private set; }

    public System.Action onHookReturn;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        hookCollider = GetComponent<Collider>(); // make sure your prefab has a collider
    }

    private void OnEnable()
    {
        IsMovingForward = true;
        IsReturning = false;
        HasCaughtEnemy = false;
        hookedEnemy = null;
        hookProgress = 0f;

        if (lineRenderer != null)
            lineRenderer.positionCount = 2;

        if (hookCollider != null)
            hookCollider.enabled = true; // enable collider when hook starts
    }

    public void SetUpHook(Transform tongue)
    {
        tongueHook = tongue;

        startPoint = tongueHook.position;
        targetPoint = startPoint + tongueHook.forward * hookRange;
        hookProgress = 0f;

        transform.position = startPoint;

        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(0, startPoint);
            lineRenderer.SetPosition(1, startPoint);
        }
    }

    private void Update()
    {
        if (tongueHook == null) return;

        if (IsMovingForward)
            MoveForward();
        else if (IsReturning)
            MoveBack();

        UpdateLine();
    }

    private void MoveForward()
    {
        hookProgress += Time.deltaTime * (hookSpeed / Vector3.Distance(startPoint, targetPoint));
        float easedProgress = Mathf.SmoothStep(0f, 1f, hookProgress);
        transform.position = Vector3.Lerp(startPoint, targetPoint, easedProgress);

        if (hookProgress >= 1f)
        {
            IsMovingForward = false;
            IsReturning = true;

            // Disable collider when it starts returning (no enemy caught)
            if (!HasCaughtEnemy && hookCollider != null)
                hookCollider.enabled = false;
        }
    }

    private void MoveBack()
    {
        Vector3 returnDir = (tongueHook.position - transform.position).normalized;
        transform.position += returnDir * returnSpeed * Time.deltaTime;

        if (hookedEnemy != null)
            hookedEnemy.transform.position = transform.position;

        // Re-enable collider once hook finishes returning
        if (Vector3.Distance(transform.position, tongueHook.position) < 0.3f)
        {
            if (hookCollider != null)
                hookCollider.enabled = true; // ready for next throw

            onHookReturn?.Invoke();
            HookPool.Instance.ReturnToPool(this);
            hookedEnemy = null;
        }
    }

    private void UpdateLine()
    {
        if (lineRenderer == null || tongueHook == null) return;

        lineRenderer.SetPosition(0, tongueHook.position);
        lineRenderer.SetPosition(1, transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsReturning) return; // ignore collisions while returning

        if (other.CompareTag("Enemy1") || other.CompareTag("Enemy2") ||
            other.CompareTag("Enemy3") || other.CompareTag("Enemy4"))
        {
            CatchEnemy(other.gameObject);
            PlayerController.instance?.TakeBar(10);
        }
    }

    private void CatchEnemy(GameObject enemyObject)
    {
        HasCaughtEnemy = true;
        hookedEnemy = enemyObject;
        IsMovingForward = false;
        IsReturning = true;

        //Disable collider once it catches something (no multiple hits)
        if (hookCollider != null)
            hookCollider.enabled = false;

        // Mark enemy as hooked
        var bee = enemyObject.GetComponent<EnemyBee>();
        if (bee != null) bee.isHooked = true;

        var fly = enemyObject.GetComponent<EnemyFly>();
        if (fly != null) fly.isHooked = true;

        var bug = enemyObject.GetComponent<EnemyBug>();
        if (bug != null) bug.isHooked = true;

        var hopper = enemyObject.GetComponent<EnemyHopper>();
        if (hopper != null) hopper.isHooked = true;
    }
}
