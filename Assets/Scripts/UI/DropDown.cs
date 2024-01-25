using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class DropDown : MonoBehaviour
{
    public Upgrades upgrades;
    public ChangeSkin skinchanger;
    public string[] skinNames;
    public TMP_Dropdown dropDown;
    public Text textBox;

    // Start is called before the first frame update
    void Start()
    {

    }
    public void UpdateList()
    {
        dropDown.options.Clear();

        List<string> items = new List<string>();
        for (int i = 0; i < upgrades.boughtSkin.Length; i++)
        {
                items.Add(skinNames[i]);
        }
        foreach (var item in items)
        {
            dropDown.options.Add(new TMP_Dropdown.OptionData() { text = item });
        }
        dropDown.value = skinchanger.skinNum;
    }

    
}
