using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUD : MonoBehaviour
{
    public TextMeshProUGUI coins,distance;
    public MovePlayer player;
    double distanceValue, convertedDistance,coinsValue,convertedCoins;
    // Update is called once per frame
    void Update()
    {
        distanceValue = player.distance;
        coinsValue = player.displayCoins;
        convertedDistance = ConvertValue(distanceValue,convertedDistance);
        convertedCoins = ConvertValue(coinsValue, convertedCoins);
        if (distanceValue >= 1000 && distanceValue < 1000000)
        {
            distance.text = convertedDistance.ToString("F2") + "km";
        }
        else if (distanceValue >= 1000000 && distanceValue < 1000000000)
        {

            distance.text = convertedDistance.ToString("F2") + "mm";
        }
        else if (distanceValue >= 1000000000)
        {

            distance.text = convertedDistance.ToString("F2") + "gm";
        }
        else if (distanceValue < 1000)
        {
            distance.text = convertedDistance.ToString() + "m";
        }

        if (coinsValue >= 1000 && coinsValue < 1000000)
        {
            coins.text = convertedCoins.ToString("F2") + "k";
        }
        else if (distanceValue >= 1000000 && distanceValue < 1000000000)
        {

            coins.text = convertedCoins.ToString("F2") + "m";
        }
        else if (coinsValue >= 1000000000)
        {

            coins.text = convertedCoins.ToString("F2") + "b";
        }
        else if (coinsValue < 1000)
        {
            coins.text = convertedCoins.ToString();
        }
    }
    public double ConvertValue(double value, double convertValue)
    {
        if (value >= 1000 && value < 1000000)
        {
            convertValue = value / 1000;
        }
        else if (value >= 1000000 && value < 1000000000)
        {
            convertValue = value / 1000000;

        }
        else if (value >= 1000000000)
        {

            convertValue = value / 1000000000;
        }
        else if (value < 1000)
        {
            convertValue = value;
        }
        return convertValue;
    }
}
