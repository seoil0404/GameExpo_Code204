using Scroll;
using UnityEngine;
using UnityEngine.UI;

public class ShopRoom_ItemView : MonoBehaviour
{
    [Header("Scriptable")]
    [SerializeField] private GoldData goldData;
    [SerializeField] private ScrollData scrollData;

    [Header("MonoBehavior")]
    [SerializeField] private ScrollView scrollView;
    [SerializeField] private Image scrollImage;
    [SerializeField] private Text priceText;

    [Header("Prefab")]
    [SerializeField] private ScrollSelectionView scrollSelectionPrefab;

    private ScrollData.ScrollType scrollType;
    private int price;

    public ScrollData.ScrollType ScrollType
    {
        get => scrollType;
        set
        {
            scrollType = value;
            scrollView.scrollType = value;
            scrollImage.sprite = scrollData.GetImage(value);
        }
    }

    public int Price
    {
        get => price;
        set
        {
            price = value;
            priceText.text = price.ToString() + "G";
#if UNITY_EDITOR
            if (value < 0) throw new System.Exception("Price lower than 0 : " + value);
#endif
        }
    }

    public void OnBuy()
    {
        if (goldData.InGameGold < price) return;

        goldData.InGameGold -= price;
        Instantiate(scrollSelectionPrefab).NewScrollType = scrollType;
        
        Destroy(gameObject);
    }
}
