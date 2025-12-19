using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class SquareTextureData : ScriptableObject
{
    [System.Serializable]
    public class TextureData
    {
        public Sprite texture;
        public Config.SquareColor squareColor;

    }
    public int tresholdVal = 10;
    public List<TextureData> activeSquareTextures;
    private const int StartTresholdVal = 10;

    public Config.SquareColor currentColor;
    private Config.SquareColor nextColor;

    public int GetCurrentColorIndex()
    {
        int currentIndex = 0;
        for (int index = 0; index < activeSquareTextures.Count; index++)
        {
            if (activeSquareTextures[index].squareColor == currentColor)
            {
                currentIndex = index;
            }
        }
        return currentIndex;
    }

    public void UpdateColors(int current_score)
    {
        currentColor = nextColor;
        int currentColorIndex = GetCurrentColorIndex();

        if (currentColorIndex == activeSquareTextures.Count -1)
        {
            nextColor = activeSquareTextures[0].squareColor;
        }
        else
        {
            nextColor = activeSquareTextures[currentColorIndex + 1].squareColor;
        }

        tresholdVal = StartTresholdVal + current_score;
    }

    public void SetStartColor()
    {
        tresholdVal = StartTresholdVal;
        currentColor = activeSquareTextures[0].squareColor;
        nextColor = activeSquareTextures[1].squareColor;
    }
    private void Awake()
    {
        SetStartColor();
    }
    void OnEnable()
    {
        SetStartColor();
    }
}
