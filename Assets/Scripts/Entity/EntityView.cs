using UnityEngine;

namespace Assets.Scripts
{
    public class EntityView : MonoBehaviour
    {
        private RectTransform _rectTransform;
        private EntityHolder _entityHolder;

        public void Setup(EntityHolder entityHolder)
        {
            _entityHolder = entityHolder;
            _rectTransform = GetComponent<RectTransform>();
            _rectTransform.SetParent(_entityHolder.RectTransform);
            _rectTransform.localPosition = Vector3.zero;
            _rectTransform.localScale = Vector3.one;
        }
    }
}