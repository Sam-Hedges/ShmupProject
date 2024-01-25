using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class ShowDistance : MonoBehaviour
{
    public TextMeshProUGUI distanceText;
    public TextMeshProUGUI coinsText;
    public double distanceValue,coinsValue;
    double convertedDistance,convertedCoins;

    //Displays the distance and coin value in the correct format depending on its value
    void Start()
    {
        distanceValue = SaveSystem.LoadData("distance");
        coinsValue = SaveSystem.LoadData("coins");
        convertedDistance = ConvertValue(distanceValue, convertedDistance);
        convertedCoins = ConvertValue(coinsValue, convertedCoins);
        if (distanceValue >= 1000 && distanceValue < 1000000)
        {
            distanceText.text = convertedDistance.ToString("F2") + "km";
        }
        else if (distanceValue >= 1000000 && distanceValue < 1000000000)
        {

            distanceText.text = convertedDistance.ToString("F2") + "mm";
        }
        else if (distanceValue >= 1000000000)
        {

            distanceText.text = convertedDistance.ToString("F2") + "gm";
        }
        else if (distanceValue < 1000)
        {
            distanceText.text = convertedDistance.ToString() + "m";
        }

        if (coinsValue >= 1000 && coinsValue < 1000000)
        {
            coinsText.text = convertedCoins.ToString("F2") + "k";
        }
        else if (distanceValue >= 1000000 && distanceValue < 1000000000)
        {

            coinsText.text = convertedCoins.ToString("F2") + "m";
        }
        else if (coinsValue >= 1000000000)
        {

            coinsText.text = convertedCoins.ToString("F2") + "b";
        }
        else if (coinsValue < 1000)
        {
            coinsText.text = convertedCoins.ToString();
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
