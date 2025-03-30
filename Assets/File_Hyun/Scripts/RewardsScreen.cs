using System.Collections.Generic;
using Scroll;
using UnityEngine;
using UnityEngine.UI;

public class RewardsScreen : MonoBehaviour
{
    public GoldData goldData;
    public Text GoldEarned;

    [Header("Scriptable")]
    [SerializeField] private ScrollData scrollData;

    [Header("Scroll")]
    [SerializeField] private Image scrollImage;
    [SerializeField] private ScrollView scrollView;

    [Header("Probability Setting")]
    [SerializeField] private float normalProbability;
    [SerializeField] private float rareProbability;
    [SerializeField] private float epicProbability;
    [SerializeField] private float legendaryProbability;

    [Header("Prefab")]
    [SerializeField] private ScrollSelectionView scrollSelectionViewPrefab;
    private ScrollData.ScrollType currentScrollType;

    private bool IsFirstAccept = true;

    private void Awake()
    {
        AllocateScroll();
    }

    private void AllocateScroll()
    {
        ScrollData.ScrollType scrollType = GetRandomScroll(ScrollData.GetScrollTypesByRarity(GetRandomScrollRarity()));

        currentScrollType = scrollType;

        scrollImage.sprite = scrollData.GetImage(scrollType);
        scrollView.scrollType = scrollType;
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

    void OnEnable()
    {
        GoldEarned.text = "+" + (goldData.InGameGold - StatisticsManager.Instance.firstGold) + "G";
    }

    public void CloseRewardsScreen()
    {
        if(IsFirstAccept)
        {
            IsFirstAccept = false;
            Instantiate(scrollSelectionViewPrefab).NewScrollType = currentScrollType;
        }
        else Scene.Controller.OnClearScene();
    }
}
