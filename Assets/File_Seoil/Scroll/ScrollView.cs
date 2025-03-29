using UnityEngine;
using UnityEngine.EventSystems;

namespace Scroll
{
    public class ScrollView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public ScrollData.ScrollType scrollType;

        [Header("Prefabs")]
        [SerializeField] private DescriptionView descriptionViewPrefab;

        [Header("Transform Setting")]
        [SerializeField] private Transform siblingTransform;

        private DescriptionView currentDescriptionView;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if(siblingTransform == null) gameObject.transform.SetAsLastSibling();
            else siblingTransform.SetAsLastSibling();

            currentDescriptionView = Instantiate(descriptionViewPrefab, transform);
            currentDescriptionView.Description.text = ScrollData.GetDescription(scrollType);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (currentDescriptionView != null) currentDescriptionView.Destroy();
        }
    }

}
