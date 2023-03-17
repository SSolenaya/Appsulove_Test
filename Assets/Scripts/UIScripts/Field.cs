using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    public class Field : BaseUIPanel, IClickReactor, IDragReactor, IReturnClickPosition, IReturnDragPosition
    {
        [Inject] private Settings _settings;
        public Action<Vector3> ActionClickPosition { get; set; }

        public Action<Vector3> ActionOnBeginDragPosition { get; set; }
        public Action<Vector3> ActionOnDragPosition { get; set; }
        public Action<Vector3> ActionOnEndDragPosition { get; set; }
        
        private void Awake()
        {
            Setup();
            ViewSetup();
        }

        private void Setup()
        {
            gameObject.AddComponent<ClickObserver>();
            gameObject.AddComponent<DragObserver>();
        }


        public bool GetRandomPosition(List<Vector3> exceptPoints, out Vector3 resultPosition)
        {
            int counter = 0;
            bool isFreePosExist = false;
            do
            {
                bool breakFlag = true;
                float x = Random.Range(_settings.fieldOffset, _rectTransform.rect.width - _settings.fieldOffset) - _rectTransform.rect.width * 0.5f;
                float y = Random.Range(_settings.fieldOffset, _rectTransform.rect.height - _settings.fieldOffset) - _rectTransform.rect.height * 0.5f;
                resultPosition = new Vector3(x, y, 0);

                foreach (Vector3 exceptPoint in exceptPoints)
                {
                    float distance = Vector3.Distance(exceptPoint, resultPosition);
                    if (distance < _settings.maximumEntitySize)
                    {
                        breakFlag = false;
                        break;
                    }
                }

                if (breakFlag)
                {
                    isFreePosExist = true;
                    break;
                }

                counter++;
            } while (counter <= 10);
            return isFreePosExist;
        }

        public void ReactionOnClick(Vector3 clickPosition)
        {
            ActionClickPosition?.Invoke(ConvertToCanvasPosition(clickPosition));
        }

        public void ReactionOnBeginDrag(Vector3 dragPosition)
        {
            ActionOnBeginDragPosition?.Invoke(ConvertToCanvasPosition(dragPosition));
        }

        public void ReactionOnDrag(Vector3 dragPosition)
        {
            ActionOnDragPosition?.Invoke(ConvertToCanvasPosition(dragPosition));
        }

        public void ReactionOnEndDrag(Vector3 dragPosition)
        {
            ActionOnEndDragPosition?.Invoke(ConvertToCanvasPosition(dragPosition));
        }

        private Vector3 ConvertToCanvasPosition(Vector3 screenPoint)
        {
            Vector3 result = _mainCanvas.Canvas.ScreenToCanvasPosition(screenPoint);
            return result;
        }
    }
}