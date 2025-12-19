using UnityEngine;
using UnityEngine.UI;

public class ActiveSquareImageSelector : MonoBehaviour
{
    [SerializeField] SquareTextureData squareTextureData;
    [SerializeField] bool updateImageOnRechedTreshold = false;

    void OnEnable()
    {
        UpdateSquareColorBaseOnCurrentPoints();
        if (updateImageOnRechedTreshold)
        {
            GameEvents.UpdateSquareColor += UpdateSquaresColor;
        }
    }
    void OnDisable()
    {
        if (updateImageOnRechedTreshold)
        {
            GameEvents.UpdateSquareColor -= UpdateSquaresColor;
        }
    }
    private void UpdateSquareColorBaseOnCurrentPoints()
    {
        foreach (var squareTexture in squareTextureData.activeSquareTextures)
        {
            if (squareTextureData.currentColor == squareTexture.squareColor)
            {
                GetComponent<Image>().sprite = squareTexture.texture;
            }
        }
    }

    private void UpdateSquaresColor(Config.SquareColor color)
    {
        foreach (var squareTexture in squareTextureData.activeSquareTextures)
        {
            if (color == squareTexture.squareColor)
            {
                GetComponent<Image>().sprite = squareTexture.texture;
            }
        }
    }
}
