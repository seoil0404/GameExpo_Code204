using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class ShopRoom_Manager : MonoBehaviour
{
    [Header("Item Views")]
    [SerializeField] private ShopRoom_ItemView[] itemViews;

    [Header("Probability Setting")]
    [SerializeField] private float normalProbability;
    [SerializeField] private float rareProbability;
    [SerializeField] private float epicProbability;
    [SerializeField] private float legendaryProbability;

    [Header("Price Setting")]
    [SerializeField] private int normalBasePrice;
    [SerializeField] private int rareBasePrice;
    [SerializeField] private int epicBasePrice;
    [SerializeField] private int legendaryBasePrice;
    [SerializeField] private int priceVariance;

    private void Awake()
    {
        AllocateScroll();
        SetScrollsPriceByRarity();
    }

    private void AllocateScroll()
    {
        do
        {
            foreach (ShopRoom_ItemView itemView in itemViews)
            {
                itemView.ScrollType = GetRandomScroll(ScrollData.GetScrollTypesByRarity(GetRandomScrollRarity()));
            }
        } while (IsSameScrollAllocated());
    }

    private ScrollData.ScrollType GetRandomScroll(ScrollData.ScrollType[] scrollTypes)
    {
        int randomIndex = Random.Range(0, scrollTypes.Length);

        if (scrollTypes[randomIndex] == ScrollData.ScrollType.Money) return GetRandomScroll(scrollTypes);

        return scrollTypes[randomIndex];
    }

    private ScrollData.ScrollRarity GetRandomScrollRarity()
    {
        float amountProbability = normalProbability + rareProbability + epicProbability + legendaryProbability;
        float randomNumber = Random.Range(0, amountProbability);

        if (randomNumber <= normalProbability) return ScrollData.ScrollRarity.Normal;
        randomNumber -= normalProbability;

        if (randomNumber <= rareProbability) return ScrollData.ScrollRarity.Rare;
        randomNumber -= rareProbability;

        if (randomNumber <= epicProbability) return ScrollData.ScrollRarity.Epic;
        randomNumber -= epicProbability;

        if (randomNumber <= legendaryProbability) return ScrollData.ScrollRarity.Legendary;
        randomNumber -= legendaryProbability;
        
#if UNITY_EDITOR
        throw new System.Exception("Probability Exception [ Amount Probability : " + amountProbability + ", Left Probability : " + randomNumber);
#else
        return ScrollData.ScrollRarity.Normal;
#endif
    }

    private bool IsSameScrollAllocated()
    {
        List<ScrollData.ScrollType> currentScrollTypes = new();

        foreach(ShopRoom_ItemView itemView in itemViews)
            if (currentScrollTypes.Contains(itemView.ScrollType)) 
                return true;

        return false;
    }

    private void SetScrollsPriceByRarity()
    {
        foreach(ShopRoom_ItemView itemView in itemViews)
            SetScrollPriceByRarity(itemView);
    }

    private void SetScrollPriceByRarity(ShopRoom_ItemView itemView)
    {
        switch (ScrollData.GetRarity(itemView.ScrollType))
        {
            case ScrollData.ScrollRarity.Normal:
                itemView.Price = normalBasePrice;
                break;
            case ScrollData.ScrollRarity.Rare:
                itemView.Price = rareBasePrice;
                break;
            case ScrollData.ScrollRarity.Epic:
                itemView.Price = epicBasePrice;
                break;
            case ScrollData.ScrollRarity.Legendary:
                itemView.Price = legendaryBasePrice;
                break;
            default:
                Debug.LogError("Unknown Rarity : " + itemView.ScrollType.ToString());
                itemView.Price = 0;
                break;
        }

        itemView.Price += Random.Range(0, priceVariance);
    }
}
