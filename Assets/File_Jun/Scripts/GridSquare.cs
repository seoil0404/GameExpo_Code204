using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSquare : MonoBehaviour
{
    public Image hooverImage;
    public Image activeImage;
    public Image normalImage;
    public List<Sprite> normalImages;
    public Sprite blueSprite;
    public Sprite redSprite;
    public Sprite yellowSprite;
    public Sprite violetSprite;
    public Sprite defaultSprite;
    private bool isPetrified = false;

    public bool Selected { get; set; }
    public int SquareIndex { get; set; }
    public bool SquareOccupied { get; set; }

    private string blockColor = "Default";
    private GameObject sealingEnemy;

    void Start()
    {
        Selected = false;
        SquareOccupied = false;
    }

    public bool CanUseThisSquare()
    {
        return hooverImage.gameObject.activeSelf;
    }

    public void DeactivateBlock()
    {
        activeImage.gameObject.SetActive(false);
        SquareOccupied = false;
        Selected = false;
    }


    public void ActivateSquare()
    {
        hooverImage.gameObject.SetActive(false);
        activeImage.gameObject.SetActive(true);
        Selected = true;
        SquareOccupied = true;
    }


    public void PlaceShapeOnBoard(string colorName)
    {
        hooverImage.gameObject.SetActive(false);
        activeImage.gameObject.SetActive(true);

        switch (colorName)
        {
            case "blue":
                activeImage.sprite = blueSprite;
                break;
            case "red":
                activeImage.sprite = redSprite;
                break;
            case "yellow":
                activeImage.sprite = yellowSprite;
                break;
            case "violet":
                activeImage.sprite = violetSprite;
                break;
            default:
                activeImage.sprite = defaultSprite;
                break;
        }

        Selected = true;
        SquareOccupied = true;
    }

    public void Deactivate()
    {
        activeImage.gameObject.SetActive(false);
    }

    public void ClearOccupied()
    {
        if (!IsSealed())
        {
            Selected = false;
            SquareOccupied = false;
        }
    }


    public void PlaceShapeOnBoard()
    {
        ActivateSquare();
    }

    public void SetOccupied()
    {
        SquareOccupied = true;
        Selected = false;
    }

    public void SetImage(bool setFirstImage)
    {
        normalImage.GetComponent<Image>().sprite = setFirstImage ? normalImages[1] : normalImages[0];
    }

    public void SetBlockColor(string color)
    {
        blockColor = color;
    }

    public string GetBlockColor()
    {
        return blockColor;
    }

    public void SealBlock(GameObject enemy)
    {
        sealingEnemy = enemy;
    }

    public bool IsSealed()
    {
        return sealingEnemy != null;
    }

    public void UnsealBlock()
    {
        sealingEnemy = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!SquareOccupied)
        {
            Selected = true;
            hooverImage.gameObject.SetActive(true);
        }
    }

    public void SetBlockSpriteToDefault()
    {
        activeImage.sprite = defaultSprite;
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        Selected = true;
        if (!SquareOccupied)
        {
            hooverImage.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!SquareOccupied)
        {
            Selected = false;
            hooverImage.gameObject.SetActive(false);
        }
    }

    public void PetrifyBlock()
    {
        SetOccupied();
        ActivateSquare();
        SetBlockSpriteToDefault();
        isPetrified = true;
    }

    public bool IsPetrified()
    {
        return isPetrified;
    }

    public void ClearPetrified()
    {
        if (isPetrified)
        {
            isPetrified = false;
            ClearOccupied();
            Deactivate();
        }
    }
}
