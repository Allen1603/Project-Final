using System.Collections.Generic;
using UnityEngine;

public class HookPool : MonoBehaviour
{
    public static HookPool Instance;

    [Header("Pool Settings")]
    public GameObject hookPrefab;
    public int poolSize = 5;

    private List<HookMechanism> hookPool = new List<HookMechanism>();

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
            GameObject hookObj = Instantiate(hookPrefab, transform);
            hookObj.SetActive(false);

            HookMechanism hook = hookObj.GetComponent<HookMechanism>();

            hookPool.Add(hook);
        }
    }

    public HookMechanism GetHook()
    {
        // Find an inactive hook in the pool
        foreach (HookMechanism hook in hookPool)
        {
            if (!hook.gameObject.activeInHierarchy)
            {
                hook.gameObject.SetActive(true);
                hook.transform.SetParent(null); // detach from pool
                return hook;
            }
        }
        return null;
    }

    public void ReturnToPool(HookMechanism hook)
    {
        if (hook == null) return;

        hook.gameObject.SetActive(false);
        hook.transform.SetParent(transform);
        hook.transform.localPosition = Vector3.zero;
        hook.transform.localRotation = Quaternion.identity;
    }
}
