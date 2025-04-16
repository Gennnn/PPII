using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HealthAndManaBars : MonoBehaviour
{
    gameManager gameManager;
    playerController playerController;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI manaText;
    [SerializeField] Image healthBackingImage;
    [SerializeField] Image manaBackingImage;
    [SerializeField] Slider healthSlider;
    [SerializeField] Slider manaSlider;

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
        manaSlider.value = (float)amount / (float)playerController.manaOrig;
        manaText.text = amount.ToString();
        manaBackingImage.color = new Color(manaBackingImage.color.r, manaBackingImage.color.g, manaBackingImage.color.b, (float)amount / (float)playerController.manaOrig);
    }

    void UpdateHealth(int amount)
    {
        healthSlider.value = (float)amount / (float)playerController.HPOrig;
        healthText.text = amount.ToString();
        healthBackingImage.color = new Color(healthBackingImage.color.r, healthBackingImage.color.g, healthBackingImage.color.b, (float)amount / (float)playerController.HPOrig);
    }
}
