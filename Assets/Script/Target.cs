using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Target : MonoBehaviour
{
    Transform[] targets;

    TextMeshProUGUI textMeshPro;

    Player player;

    public float hp = 100.0f;

    Transform hittedTarget;

    public float HP
    {
        get { return hp; }
    }


    private void Awake()
    {
    }

    private void Start()
    {
        GameObject canvasObject = GameObject.FindGameObjectWithTag("Canvas");
        Canvas canvas = canvasObject.GetComponent<Canvas>();
        Transform canvasChild = canvas.transform.GetChild(2);
        textMeshPro = canvasChild.GetComponent<TextMeshProUGUI>();

        player = FindAnyObjectByType<Player>();
        //player = GetComponent<Player>();

        textMeshPro.enabled = false;
    }

    private void Update()
    {
        if(player.Hit)
        {
            textMeshPro.transform.position =  transform.position;
            textMeshPro.enabled = true;
        }
    }

    IEnumerator HitText()
    {
        while (true)
        {
            textMeshPro.transform.position = hittedTarget.position;
            textMeshPro.enabled = true;
            yield return new WaitForSeconds(1.0f);
            textMeshPro.enabled = false;
        }
    }
}
