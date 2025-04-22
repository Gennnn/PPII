using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class HealthAndManaBars : MonoBehaviour
{
    gameManager gameManager;
    playerController playerController;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI manaText;
    [SerializeField] Slider healthSlider;
    [SerializeField] Slider manaSlider;

    Tween healthValueTween;
    Tween manaValueTween;

    [SerializeField] float tweenSpeed = 0.25f;

    void Awake()
    {
        
    }

    private void Start()
    {
        gameManager = gameManager.instance;
        playerController = gameManager.playerScript;
        playerController.manaUpdatedEvent.AddListener(UpdateMana);
        playerController.hpUpdatedEvent.AddListener(UpdateHealth);
    }

    private void OnDisable()
    {
        playerController.manaUpdatedEvent.RemoveListener(UpdateMana);
        playerController.hpUpdatedEvent.RemoveListener(UpdateHealth);
    }

    void UpdateMana(int amount)
    {
        if (manaValueTween != null)
        {
            manaValueTween.Kill(false);
        }
        manaValueTween = manaSlider.DOValue((float)amount / (float)playerController.manaOrig, tweenSpeed).OnComplete( () =>
        {
            manaValueTween = null;
        });
        
        manaText.text = amount.ToString() +"/" + playerController.manaOrig;
    }

    void UpdateHealth(int amount)
    {
        if (healthValueTween != null)
        {
            healthValueTween.Kill(false);
        }
        healthValueTween = healthSlider.DOValue((float)amount / (float)playerController.HPOrig, tweenSpeed).OnComplete(() =>
        {
            healthValueTween = null;
        });
        healthText.text = amount.ToString() +"/" + playerController.HPOrig;
    }
}
