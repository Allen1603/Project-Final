using System.Collections;
using UnityEngine;

public class FunFact : MonoBehaviour
{
    public GameObject[] funfactTXT;
    private int lastIndex = -1;

    private void Start()
    {
        // Hide all facts at the start
        foreach (GameObject txt in funfactTXT)
            txt.SetActive(false);

        StartCoroutine(RandomFunfact());
    }

    private IEnumerator RandomFunfact()
    {
        while (true)
        {
            // pick a new index different from the last one
            int index;
            do   
            {
                index = Random.Range(0, funfactTXT.Length);
            }
            while (index == lastIndex);

            lastIndex = index;

            // hide all
            foreach (GameObject txt in funfactTXT)
                txt.SetActive(false);

            // show the chosen one
            funfactTXT[index].SetActive(true);

            yield return new WaitForSeconds(5f);
        }
    }
}
