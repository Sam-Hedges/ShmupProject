using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourLerp : MonoBehaviour
{
    public Material meshRenderer;
    [SerializeField] [Range(0f, 1f)] float lerpTime;
    [SerializeField] Color[] myColours;

    int colourIndex = 0;

    float t = 0f;

    int len;
    // Start is called before the first frame update
    void Start()
    {
        len = myColours.Length;
    }

    // Update is called once per frame
    void Update()
    {
        meshRenderer.color = Color.Lerp(meshRenderer.color, myColours[colourIndex], lerpTime * Time.deltaTime);
        meshRenderer.SetColor("_EmissionColor", Color.Lerp(meshRenderer.color, myColours[colourIndex], lerpTime * Time.deltaTime));
        t = Mathf.Lerp(t, 1f, lerpTime * Time.deltaTime);
        if (t > .9){
            t = 0f;
            colourIndex++;
            colourIndex = (colourIndex >= len) ? 0 : colourIndex;
        }
    }
}
