using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    public float alphaChangeSpeed = 1.0f;

    Player player;

    TextMeshProUGUI hitRateText;

    Button restartButton;

    Restart restart;

    GeneralUI generalUI;
    CanvasGroup canvasGroup;

    float hitRate;

    private void Awake()
    {
        generalUI = GetComponentInParent<GeneralUI>();
        canvasGroup = GetComponent<CanvasGroup>();

        Transform child = transform.GetChild(0);
        hitRateText = child.GetComponent<TextMeshProUGUI>();    // HitRate

        child = transform.GetChild (1);
        restartButton = child.GetComponent<Button>();           // Restart Button]

        restart = child.GetComponent<Restart>();
    }

    private void Start()
    {
        player = FindFirstObjectByType<Player>();
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        generalUI.onEndTimer = () => { StartCoroutine(IncreaseAlpha()); };
        generalUI.onEndTimer += GameOver;

        restart.onTurnOffOption += TurnOff;
    }


    void GameOver()
    {
        player.OnOption(true);
        hitRate = ((player.HitCount * 100.0f) / (player.ShootCount * 100.0f)) * 100.0f;
        Debug.Log(hitRate);
        hitRateText.text = $"Hit Rate : {hitRate:f2}%\n\r{player.HitCount} / {player.ShootCount}";
    }

    void TurnOff()
    {
        canvasGroup.alpha = 0;
        player.OnOption(false);
    }

    IEnumerator IncreaseAlpha()
    {
        while (canvasGroup.alpha < 1.0f)
        {
            canvasGroup.alpha += Time.deltaTime * alphaChangeSpeed;
            generalUI.onEndTimer -= GameOver;
            yield return null;
        }
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

}
