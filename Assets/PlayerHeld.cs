using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeld : MonoBehaviour
{
    gameManager gameManager;
    playerController playerController;

    int currentHeldItem = 0;
    [SerializeField] Item[] items = new Item[3];
    
    void Start()
    {
        gameManager = gameManager.instance;
        playerController = gameManager.playerScript;
        playerController.swapSlot.AddListener(SwapItem);
        playerController.playerSwing.AddListener(Swing);
    }

    private void OnDisable()
    {
        playerController.swapSlot.RemoveListener(SwapItem);
        playerController.playerSwing.RemoveListener(Swing);
    }

    void SwapItem(int slot)
    {
        items[currentHeldItem].gameObject.SetActive(false);
        currentHeldItem = slot;
        items[currentHeldItem].gameObject.SetActive(true);
    }

    void Swing(int type)
    {
        if (type == 0)
        {
            items[currentHeldItem].Primary();
        } else
        {
            items[currentHeldItem].Secondary();
        }
    }
    
    void Update()
    {
        
    }
}
