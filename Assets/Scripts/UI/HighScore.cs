using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScore : MonoBehaviour
{
    public TextMeshProUGUI highscore, currentDistance;
    public double currentDistanceValue,convertedDistance;
    // Start is called before the first frame update
    void Start()
    {
        currentDistanceValue = VariableTransfer.currentDistance;
        convertedDistance = ConvertValue(currentDistanceValue, convertedDistance);
        if (currentDistanceValue >= 1000 && currentDistanceValue < 1000000)
        {
            currentDistance.text = "Your Distance: " + convertedDistance.ToString("F2") + "km";
        }
        else if (currentDistanceValue >= 1000000 && currentDistanceValue < 1000000000)
        {

            currentDistance.text = "Your Distance: " + convertedDistance.ToString("F2") + "mm";
        }
        else if (currentDistanceValue >= 1000000000)
        {

            currentDistance.text = "Your Distance: " + convertedDistance.ToString("F2") + "gm";
        }
        else if (currentDistanceValue < 1000)
        {
            currentDistance.text = "Your Distance: " + convertedDistance.ToString() + "m";
        }

        if (SaveSystem.highscore == true)
        {
            highscore.gameObject.SetActive(true);
        }
        else
        {
            highscore.gameObject.SetActive(false);
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
