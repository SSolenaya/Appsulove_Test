using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Assets.Scripts
{

    public abstract class BaseUIPanel : MonoBehaviour
    {
        [Inject] protected MainCanvas _mainCanvas;
        protected RectTransform _rectTransform;

        protected void ViewSetup()
        {
            _rectTransform = GetComponent<RectTransform>();
            _rectTransform.SetParent(_mainCanvas.CanvasParent);
            _rectTransform.localScale = Vector3.one;
            _rectTransform.offsetMax = Vector2.zero;
            _rectTransform.offsetMin = Vector2.zero;
        }
    }
}
