using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorShift : MonoBehaviour
{
    public float firstHue;
    public float secondHue;
    public float thirdHue;
    public float fourthHue;
    public float saturation;
    public float value;
    public float rate;

    public Material background;
    private Texture2D texture;

    void Start()
    {
        texture = new Texture2D(2, 2);
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.filterMode = FilterMode.Bilinear;

        texture.SetPixel(0, 0, Color.HSVToRGB(firstHue, saturation, value));
        texture.SetPixel(1, 0, Color.HSVToRGB(secondHue, saturation, value));
        texture.SetPixel(0, 1, Color.HSVToRGB(thirdHue, saturation, value));
        texture.SetPixel(1, 1, Color.HSVToRGB(fourthHue, saturation, value));
        texture.Apply();
        background.mainTexture = texture;
    }

    void Update()
    {
        texture.SetPixel(0, 0, Color.HSVToRGB(firstHue, saturation, value));
        texture.SetPixel(1, 0, Color.HSVToRGB(secondHue, saturation, value));
        texture.SetPixel(0, 1, Color.HSVToRGB(thirdHue, saturation, value));
        texture.SetPixel(1, 1, Color.HSVToRGB(fourthHue, saturation, value));
        texture.Apply();
        // background.mainTexture = texture;

        firstHue += rate * Time.deltaTime;
        if (firstHue > 1) firstHue = 0;
        secondHue += rate * Time.deltaTime;
        if (secondHue > 1) secondHue = 0;
        thirdHue += rate * Time.deltaTime;
        if (thirdHue > 1) thirdHue = 0;
        fourthHue += rate * Time.deltaTime;
        if (fourthHue > 1) fourthHue = 0;
    }
}
