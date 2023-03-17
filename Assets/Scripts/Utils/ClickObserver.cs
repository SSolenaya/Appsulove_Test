using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    [RequireComponent(typeof(IClickReactor))]
    public class ClickObserver : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private IClickReactor _clickReactor;

        private void Awake()
        {
            if (_clickReactor == null)
            {
                _clickReactor = GetComponent<IClickReactor>();
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _clickReactor.ReactionOnClick(eventData.position);
        }
    }
}