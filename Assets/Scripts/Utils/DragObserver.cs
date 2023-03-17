using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    [RequireComponent(typeof(IDragReactor))]
    public class DragObserver : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private IDragReactor _dragReactor;

        private void Awake()
        {
            if (_dragReactor == null)
            {
                _dragReactor = GetComponent<IDragReactor>();
            }
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            _dragReactor.ReactionOnBeginDrag(eventData.position);
        }

        public void OnDrag(PointerEventData eventData)
        {
            _dragReactor.ReactionOnDrag(eventData.position);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _dragReactor.ReactionOnEndDrag(eventData.position);
        }
    }
}