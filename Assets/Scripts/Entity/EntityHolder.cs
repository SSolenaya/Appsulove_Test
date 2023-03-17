using UnityEngine;

namespace Assets.Scripts
{
    public class EntityHolder : MonoBehaviour
    {
        private RectTransform _rectTransform;

        public RectTransform RectTransform
        {
            get
            {
                if (_rectTransform == null)
                {
                    _rectTransform = GetComponent<RectTransform>();
                }

                return _rectTransform;
            }
        }

        public void Setup(RectTransform parent, string nameEntity = null)
        {
            if (nameEntity != null)
            {
                gameObject.name = nameEntity;
            }

            RectTransform.SetParent(parent);
            RectTransform.localScale = Vector3.one;
        }

        public void SetPosition(Vector3 newPosition)
        {
            RectTransform.anchoredPosition3D = newPosition;
        }
    }
}