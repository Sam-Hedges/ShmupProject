using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSkin : MonoBehaviour
{
    public MeshRenderer[] mesh = new MeshRenderer[9];
    public Material[] defaultSkin;
    public Material[] transparentSkin;
    public Material[] pinkSkin;
    public Material[] midnight;
    public Material[] purple;
    public Material[] gold;
    public int skinNum;
    public string skinName;
    // Start is called before the first frame update

    public void Start()
    {
        skinNum = SaveSystem.LoadData("skin");
        SkinChange(skinNum);
    }

    public void SkinChange(int skinNum)
    {
        switch (skinNum)
        {
            case 0:
                ApplyMaterial(defaultSkin);
                skinName = "Default";
                break;
            case 1:
                ApplyMaterial(transparentSkin);
                skinName = "Transparent";
                break;
            case 2:
                ApplyMaterial(pinkSkin);
                skinName = "Pink";
                break;
            case 3:
                ApplyMaterial(purple);
                skinName = "Purple";
                break;
            case 4:
                ApplyMaterial(midnight);
                skinName = "Midnight";
                break;
            case 5:
                ApplyMaterial(gold);
                skinName = "Gold";
                break;
        }
    }
    public void ApplyMaterial(Material[] skin)
    {
        mesh[0].material = skin[1];
        mesh[1].material = skin[4];
        mesh[2].material = skin[2];
        mesh[3].material = skin[2];
        mesh[4].material = skin[3];
        mesh[5].material = skin[2];
        mesh[6].material = skin[2];
        mesh[7].material = skin[1];
        mesh[8].material = skin[0];
    }
}
