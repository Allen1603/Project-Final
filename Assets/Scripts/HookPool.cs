using System.Collections.Generic;
using UnityEngine;

public class HookPool : MonoBehaviour
{
    public static HookPool Instance;

    [Header("Hook Prefabs for Each Character")]
    public GameObject[] tongueHookPrefabs;  // 0 = char1, 1 = char2, 2 = char3
    public int poolSize = 5;

    // Separate pools per character
    private List<HookMechanism>[] hookPools;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        InitializePools();
    }

    void InitializePools()
    {
        // Create pool arrays
        hookPools = new List<HookMechanism>[tongueHookPrefabs.Length];

        for (int i = 0; i < tongueHookPrefabs.Length; i++)
        {
            hookPools[i] = new List<HookMechanism>();

            for (int j = 0; j < poolSize; j++)
            {
                GameObject hookObj = Instantiate(tongueHookPrefabs[i], transform);
                hookObj.SetActive(false);

                HookMechanism hook = hookObj.GetComponent<HookMechanism>();
                hookPools[i].Add(hook);
            }
        }
    }

    // Selects which pool to use based on character index
    public HookMechanism GetHook(int characterIndex)
    {
        if (characterIndex < 0 || characterIndex >= hookPools.Length)
        {
            Debug.LogError("Invalid character index!");
            return null;
        }

        List<HookMechanism> selectedPool = hookPools[characterIndex];

        foreach (HookMechanism hook in selectedPool)
        {
            if (!hook.gameObject.activeInHierarchy)
            {
                hook.gameObject.SetActive(true);
                hook.transform.SetParent(null);
                return hook;
            }
        }

        return null; // all hooks in use
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
