using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Restart : MonoBehaviour
{
    GeneralUI generalUI;

    Button restart;

    Player player;

    public Action onTurnOffOption;

    private void Awake()
    {
        restart = GetComponent<Button>();
        Transform parent = transform.parent;
        parent = parent.parent;
        generalUI = parent.GetComponent<GeneralUI>();
    }

    private void Start()
    {
        player = FindFirstObjectByType<Player>();
    }

    private void OnEnable()
    {
        restart.onClick.AddListener(() => OnClick());
    }

    void OnClick()
    {
        player.Initialize();
        generalUI.Initialize();
        onTurnOffOption?.Invoke();
    }
}
