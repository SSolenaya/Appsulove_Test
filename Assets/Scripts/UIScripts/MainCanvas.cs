using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{

    public class MainCanvas : MonoBehaviour
    {
        private RectTransform _canvasParent;

        public RectTransform CanvasParent
        {
            get
            {
                if (_canvasParent == null)
                {
                    _canvasParent = GetComponent<RectTransform>();
                }

                return _canvasParent;
            }
        }

        private Canvas _canvas;

        public Canvas Canvas {
            get {
                if (_canvas == null)
                {
                    _canvas = GetComponent<Canvas>();
                }

                return _canvas;
            }
        }
    }
}
