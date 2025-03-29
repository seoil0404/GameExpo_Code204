using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopRoom_UltimateItemView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Scriptable")]
    [SerializeField] private GoldData goldData;

    [Header("Prefabs")]
    [SerializeField] private DescriptionView descriptionViewPrefab;

    [Header("Transform")]
    [SerializeField] private Transform siblingTransform;

    [Header("MonoBehavior")]
    [SerializeField] private Text priceText;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip buySound;

    private DescriptionView currentDescriptionView;

    private static int price = 100;

    private void Awake()
    {
        priceText.text = price.ToString() + "G";
    }

    public void OnBuy()
    {
        if (goldData.InGameGold < price) return;

        goldData.InGameGold -= price;
        price = (int)(price *1.2);

        priceText.text = price.ToString() + "G";

        CharacterManager.selectedCharacter.characterData.MaxUltimateGauge -= 2;

        Scene.Controller.audioSource.clip = buySound;
        Scene.Controller.audioSource.Play();

        if (CharacterManager.selectedCharacter.characterData.CurrentUltimateGauge > CharacterManager.selectedCharacter.characterData.MaxUltimateGauge)
            CharacterManager.selectedCharacter.characterData.CurrentUltimateGauge = CharacterManager.selectedCharacter.characterData.MaxUltimateGauge;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (siblingTransform == null) gameObject.transform.SetAsLastSibling();
        else siblingTransform.SetAsLastSibling();

        currentDescriptionView = Instantiate(descriptionViewPrefab, transform);

        currentDescriptionView.Description.text = "궁극기 강화\n\n캐릭터의 궁극기를 강화하여 궁극기 사용시 필요 턴이 2 감소한다.";
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (currentDescriptionView != null) currentDescriptionView.Destroy();
    }
}
