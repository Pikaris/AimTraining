using System;
using System.Collections;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public GameObject targetCircle;

    GameObject hittedObj;


    public float minX = -35.0f;
    public float maxX = 35.0f;
    public float minY = 2.0f;
    public float maxY = 30.0f;
    public float minZ = 50.0f;
    public float maxZ = 60.0f;
    public int maxTarget = 15;

    private void Awake()
    {
        for(int i = 0; i < maxTarget; i++)
        {
            SetRandomTarget();
        }
    }

    private void Start()
    {
    }

    public void SetHittedTarget(GameObject obj)
    {
        hittedObj = obj;
        hittedObj.SetActive(false);
        hittedObj.transform.position = RandomPosition();
        hittedObj.SetActive(true);
    }

    private void SetRandomTarget()
    {
        if(targetCircle != null)
        {
            targetCircle.transform.position = RandomPosition();

            Instantiate(targetCircle);
        }
    }

    private Vector3 RandomPosition()
    {
        return new(UnityEngine.Random.Range(minX, maxX), UnityEngine.Random.Range(minY, maxY), UnityEngine.Random.Range(minZ, maxZ));
    }
}
