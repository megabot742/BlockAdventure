using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GridSquare : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] public Image normalImage;
    [SerializeField] Image hoverImage;
    [SerializeField] Image activeImage;
    [SerializeField] public List<Sprite> normalImageList;

    private Config.SquareColor currentSquareColor = Config.SquareColor.NotSet;

    public Config.SquareColor GetCurrentColor()
    {
        return currentSquareColor;
    }
    public bool Selected { get; set; }
    public int SquareIndex { get; set; }
    public bool SquraeOccupied { get; set; }
    void Start()
    {
        Selected = false;
        SquraeOccupied = false;
    }
    //temp funcction, remove it;
    public bool CanWeUseThisSquare()
    {
        return hoverImage.gameObject.activeSelf;
    }
    public void PlaceShapeOnBoard(Config.SquareColor color)
    {
        currentSquareColor = color;
        ActivateSquare();
    }
    public void ActivateSquare()
    {
        hoverImage.gameObject.SetActive(false);
        activeImage.gameObject.SetActive(true);
        Selected = true;
        SquraeOccupied = true;
    }
    public void DeactivateSquare()
    {
        currentSquareColor = Config.SquareColor.NotSet;
        activeImage.gameObject.SetActive(false);
    }
    public void ClearOccupied()
    {
        Selected = false;
        SquraeOccupied = false;
        currentSquareColor = Config.SquareColor.NotSet;
    }
    public void SetImage(bool setFirstImage)
    {
        normalImage.GetComponent<Image>().sprite = setFirstImage ? normalImageList[1] : normalImageList[0];
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (SquraeOccupied == false)
        {
            Selected = true;
            hoverImage.gameObject.SetActive(true);
        }
        else if(collision.GetComponent<ShapeSquare>() != null)
        {
            collision.GetComponent<ShapeSquare>().SetOccupied();
        }
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        Selected = true;
        if (SquraeOccupied == false)
        {
            hoverImage.gameObject.SetActive(true);
        }
        else if(collision.GetComponent<ShapeSquare>() != null)
        {
            collision.GetComponent<ShapeSquare>().SetOccupied();
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (SquraeOccupied == false)
        {
            Selected = false;
            hoverImage.gameObject.SetActive(false);
        }
        else if(collision.GetComponent<ShapeSquare>() != null)
        {
            collision.GetComponent<ShapeSquare>().UnSetOccupied();
        }
    }
}
