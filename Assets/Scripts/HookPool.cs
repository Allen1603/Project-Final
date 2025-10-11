using System.Collections.Generic;
using UnityEngine;

public class HookPool : MonoBehaviour
{
    public static HookPool Instance;

    [Header("Pool Settings")]
    public GameObject hookPrefab;
    public int poolSize = 5;

    private Queue<HookMechanism> hookPool = new Queue<HookMechanism>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        InitializePool();
    }

    void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            CreateAndEnqueueHook();
        }
    }

    private void CreateAndEnqueueHook()
    {
        GameObject hookObj = Instantiate(hookPrefab, transform);
        hookObj.SetActive(false);
        HookMechanism hook = hookObj.GetComponent<HookMechanism>();

        hookPool.Enqueue(hook);
    }

    public HookMechanism GetHook()
    {
        if (hookPool.Count == 0)
        {
            // Auto-expand pool safely
            CreateAndEnqueueHook();
        }

        HookMechanism hook = hookPool.Dequeue();
        hook.gameObject.SetActive(true);

        // Optionally reset hook position or state
        hook.transform.SetParent(null);

        return hook;
    }

    public void ReturnToPool(HookMechanism hook)
    {
        hook.gameObject.SetActive(false);
        hook.transform.SetParent(transform);

        // Optionally reset velocity or state here
        hookPool.Enqueue(hook);
    }
}
