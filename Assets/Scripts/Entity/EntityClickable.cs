using System;
using UniRx;
using UnityEngine;

namespace Assets.Scripts
{
    public class EntityClickable : MonoBehaviour, IClickReactor
    {
        public ReactiveCommand CommandOnClick = new ReactiveCommand();
        protected RectTransform _rectTransform;
        protected EntityHolder _entityHolder;

        public void Setup(EntityHolder entityHolder)
        {
            _entityHolder = entityHolder;
            _rectTransform = GetComponent<RectTransform>();
            _rectTransform.SetParent(_entityHolder.RectTransform);
            _rectTransform.localPosition = Vector3.zero;

            gameObject.AddComponent<ClickObserver>();
        }

        public void ReactionOnClick(Vector3 clickPosition)
        {
            CommandOnClick.Execute();
        }
    }
}