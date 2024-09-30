using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HitCount : MonoBehaviour
{
    TextMeshProUGUI hitCount;
    Player player;

    private void Awake()
    {
        hitCount = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        player = FindFirstObjectByType<Player>();

        player.onHitChange += HitCountChange;
        hitCount.text = $"Count : 0";
    }

    private void HitCountChange()
    {
        hitCount.text = $"Count : {player.HitCount}";
    }
}
