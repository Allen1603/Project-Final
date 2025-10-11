using UnityEngine;

public class HookMechanism : MonoBehaviour
{
    [Header("Hook Settings")]
    public float speed;
    public float maxDistance;

    private Vector3 startPoint;
    private Vector3 targetPoint;
    private float hookProgress = 0f;

    private Transform playerTransform;
    private GameObject hookedEnemy;
    private LineRenderer lineRenderer;

    // States
    public bool IsReturning { get; private set; }
    public bool IsMovingForward { get; private set; }
    public bool HasCaughtEnemy { get; private set; }

    public System.Action onHookReturn;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void OnEnable()
    {
        // Reset all states when reactivated from pool
        IsMovingForward = true;
        IsReturning = false;
        HasCaughtEnemy = false;
        hookedEnemy = null;
        hookProgress = 0f;

        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 2;
        }
    }

    public void SetUpHook(Transform player)
    {
        playerTransform = player;
        startPoint = player.position;
        speed = PlayerController.instance.hookSpeed;
        maxDistance = PlayerController.instance.hookRange;

        // Aim hook forward (in joystick direction)
        targetPoint = startPoint + player.forward * maxDistance;
        hookProgress = 0f;

        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(0, startPoint);
            lineRenderer.SetPosition(1, startPoint);
        }
    }

    private void Update()
    {
        if (playerTransform == null) return;

        if (IsMovingForward)
        {
            MoveForward();
        }
        else if (IsReturning)
        {
            MoveBack();
        }

        UpdateLine();
    }

    //Move forward until reaching maxDistance
    private void MoveForward()
    {
        hookProgress += Time.deltaTime * (speed / Vector3.Distance(startPoint, targetPoint));
        float easedProgress = Mathf.SmoothStep(0f, 1f, hookProgress);
        transform.position = Vector3.Lerp(startPoint, targetPoint, easedProgress);

        if (hookProgress >= 1f)
        {
            IsMovingForward = false;
            IsReturning = true;
        }
    }

    // Move back to player, dragging hooked enemy
    private void MoveBack()
    {
        Vector3 returnDir = (playerTransform.position - transform.position).normalized;
        transform.position += returnDir * speed * Time.deltaTime;

        if (hookedEnemy != null)
            hookedEnemy.transform.position = transform.position;

        if (Vector3.Distance(transform.position, playerTransform.position) < 1f)
        {
            onHookReturn?.Invoke();

            if (HookPool.Instance != null)
                HookPool.Instance.ReturnToPool(this);
            else
                gameObject.SetActive(false); // fallback safe

            hookedEnemy = null;
        }
    }

    private void UpdateLine()
    {
        if (lineRenderer == null || playerTransform == null) return;

        lineRenderer.SetPosition(0, playerTransform.position);
        lineRenderer.SetPosition(1, transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsReturning) return; // ignore collisions while returning

        if (other.CompareTag("Enemy1"))
        {
            CatchEnemy(other.gameObject);
            PlayerController.instance?.TakeBar(10);
        }
        if (other.CompareTag("Enemy2"))
        {
            CatchEnemy(other.gameObject);
            PlayerController.instance?.TakeBar(10);
        }
        if (other.CompareTag("Enemy3"))
        {
            CatchEnemy(other.gameObject);
            PlayerController.instance?.TakeBar(10);
        }
        else if (other.CompareTag("Enemy4"))
        {
            CatchEnemy(other.gameObject);
            PlayerController.instance?.TakeExp(10);
        }
    }

    private void CatchEnemy(GameObject enemyObject)
    {
        HasCaughtEnemy = true;
        hookedEnemy = enemyObject;
        IsMovingForward = false;
        IsReturning = true;

        // Mark as hooked for behavior control
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
