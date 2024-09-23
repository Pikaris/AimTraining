using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Target : TargetManager
{
    private TextMeshPro textMeshPro;

    Coroutine setTextCoroutine;

    bool hitted;

    public bool Hitted
    {
        set
        {
            hitted = value;

            if (setTextCoroutine != null)
            {
                if (hitted)
                {
                    transform.position = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), Random.Range(minZ, maxZ));
                    gameObject.SetActive(true);
                    setTextCoroutine = StartCoroutine(SetTextCoroutine());
                }
                else
                {
                    StopCoroutine(setTextCoroutine);
                }
            }
        }
    }

    private void Awake()
    {
        textMeshPro = transform.GetChild(0).GetComponent<TextMeshPro>();

        textMeshPro.enabled = false;
    }

    IEnumerator SetTextCoroutine()
    {
        while(hitted)
        {
            textMeshPro.enabled = true;
            yield return new WaitForSeconds(0.5f);
            textMeshPro.enabled = false;
        }
    }
}
