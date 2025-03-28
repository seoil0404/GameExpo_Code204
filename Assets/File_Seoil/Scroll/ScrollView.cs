using UnityEngine;
using UnityEngine.EventSystems;

namespace Scroll
{
    public class ScrollView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public ScrollData.ScrollType scrollType;

        [Header("Prefabs")]
        [SerializeField] private DescriptionView descriptionViewPrefab;

        private DescriptionView currentDescriptionView;

        public void OnPointerEnter(PointerEventData eventData)
        {
            gameObject.transform.SetAsLastSibling();

            currentDescriptionView = Instantiate(descriptionViewPrefab, transform);
            currentDescriptionView.Description.text = ScrollData.GetDescription(scrollType);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (currentDescriptionView != null) currentDescriptionView.Destroy();
        }
    }

}
