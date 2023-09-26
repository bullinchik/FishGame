using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BitePower : MonoBehaviour
{
    public TextMeshProUGUI money;
    public TextMeshProUGUI bitePower;
    public GameObject player;
    public GameObject inactiveSprite;
    void Start()
    {
        money.text = UserResources.money.ToString();
        bitePower.text = UserResources.basicAttack.ToString() + " LVL";
        if (UserResources.money < 20) inactiveSprite.SetActive(true);
        else inactiveSprite.SetActive(false);
    }
    
    public void UpgradePower()
    {
        if (UserResources.money >= 20)
        {
            UserResources.UpdateBasicAttack(1);
            player.GetComponent<EatingFood>().Start();
            UserResources.AddMoney(-20); 
        }
        
        money.text = UserResources.money.ToString();
        bitePower.text = UserResources.basicAttack.ToString() + " LVL";

        if (UserResources.money < 20) inactiveSprite.SetActive(true);
        else inactiveSprite.SetActive(false);
    }
}
