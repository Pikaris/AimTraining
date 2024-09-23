using System.Collections;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public GameObject targetCircle;

    Player player;

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
        player = FindFirstObjectByType<Player>();
    }

    private void Update()
    {
        if(player.Hit)
        {
            //SetRandomTarget();
        }
    }

    private void SetRandomTarget()
    {
        if(targetCircle != null)
        {
            targetCircle.transform.position =
                new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), Random.Range(minZ, maxZ));

            Instantiate(targetCircle);
        }
    }
}
