using System;
using UnityEngine;

namespace Assets.Scripts
{
    public interface IReturnDragPosition
    {
        public Action<Vector3> ActionOnBeginDragPosition { get; set; }
        public Action<Vector3> ActionOnDragPosition { get; set; }
        public Action<Vector3> ActionOnEndDragPosition { get; set; }
    }
}