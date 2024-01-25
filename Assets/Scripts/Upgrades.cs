using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Upgrades : MonoBehaviour
{
    public TextMeshProUGUI coinsText,speedText,CostText,nitroCostText,noCoins,buttonText,nitroText;
    public bool[] boughtSkin;
    public int[] skinCost;
    public int speedLvl = 1;
    public int speedCost = 100;
    public int nitroLvl = 1;
    public int nitroCost = 200;
    int coins;
    public ChangeSkin skinChanger;
    public DropDown dropDown;
    public int selectedSkin,currentSkin;
    void Start()
    {
        noCoins.text = "";
        coins = SaveSystem.LoadData("coins");
        coinsText.text = coins.ToString();
        SaveSystem.LoadShop(this);
        currentSkin = SaveSystem.LoadData("skin");
        dropDown.UpdateList();
        speedText.text = "Speed Level: " + speedLvl;
        CostText.text = "Cost: " + speedCost;
        nitroText.text = "Boost Level: " + nitroLvl;
        nitroCostText.text = "Cost: " + nitroCost;
        boughtSkin[0] = true;
        dropDown.dropDown.value = currentSkin;
        HandleInputData(currentSkin);
    }
    public void SpeedUpgrade()
    {
        costCalculator(speedCost, speedLvl);

        if (coins >= speedCost && speedLvl != 5)
        {
            VariableTransfer.speed += 2;
            coins -= speedCost;
            speedLvl++;
            speedCost *= speedLvl;
            coinsText.text = coins.ToString();
            speedText.text = "Speed Level: " + speedLvl;
            CostText.text = "Cost: " + speedCost;

        }
        else if (coins < speedCost)
        {
            StartCoroutine(Duration(CostText, "Not enough Coins", "Cost: " + speedCost));
        }
        else if (speedLvl == 5)
        {
            CostText.text = "Maxed out"; 
        }

    }
    public void NitroUpgrade()
    {
        costCalculator(nitroCost, nitroLvl);

        if (coins >= nitroCost && nitroLvl != 5)
        {
            VariableTransfer.maxDuration += 2;
            coins -= nitroCost;
            nitroLvl++;
            nitroCost *= nitroLvl;
            coinsText.text = coins.ToString();
            nitroText.text = "Boost Level: " + nitroLvl;
            nitroCostText.text = "Cost: " + nitroCost;

        }
        else if (coins < nitroCost)
        {
            StartCoroutine(Duration(nitroCostText, "Not enough Coins", "Cost: " + nitroCost));
        }
        else if (nitroLvl == 5)
        {
            nitroCostText.text = "Maxed out";
        }
    }
    public void HandleInputData(int val)
    {
        selectedSkin = val;
        skinChanger.SkinChange(val);
        if (currentSkin == val)
        {
            buttonText.text = "Equipped";
        }
        else if (boughtSkin[val] == true)
        {
            buttonText.text = "Equip Skin";
        }
        else
        {
            buttonText.text = "Buy!\nCost: " + skinCost[val] ;
        }
    }
    public void BuySkin()
    {
        if (boughtSkin[selectedSkin] == false)
        {
            if (coins >= skinCost[selectedSkin])
            {
                boughtSkin[selectedSkin] = true;
                coins -= skinCost[selectedSkin];
                currentSkin = selectedSkin;
                VariableTransfer.skinnum = currentSkin;
                skinChanger.skinNum = currentSkin;
                buttonText.text = "Equipped";
                coinsText.text = coins.ToString();
            }
            else
            {
                    StartCoroutine(Duration(buttonText, "Not enough Coins", "Buy!\nCost: " + skinCost[selectedSkin]));

            }
        }
        else
        {
            currentSkin = selectedSkin;
            VariableTransfer.skinnum = selectedSkin;
            skinChanger.skinNum = selectedSkin;
            buttonText.text = "Equipped";
        }
        
    }
    public IEnumerator Duration(TextMeshProUGUI text, string str, string strEnd)
    {
        text.text = str;
        yield return new WaitForSeconds(3);
        text.text = strEnd;

    }
    public void OnClose()
    {
        if(currentSkin != selectedSkin)
        {
            skinChanger.SkinChange(SaveSystem.LoadData("skin"));
            dropDown.dropDown.value = currentSkin;
        }
        SaveSystem.SavePlayer(VariableTransfer.speed,VariableTransfer.maxDuration);
        SaveSystem.SaveData(VariableTransfer.skinnum, "skin");
        SaveSystem.SaveData(coins,"coins");
        SaveSystem.SaveShop(speedLvl, speedCost,boughtSkin,nitroLvl,nitroCost);
    }

    void costCalculator(float cost, int lvl)
    {
        cost *= lvl;
    }
}
