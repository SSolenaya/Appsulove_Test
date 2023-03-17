using System;
using UnityEngine;

namespace Assets.Scripts
{
    public interface IReturnClickPosition
    {
        public Action<Vector3> ActionClickPosition { get; set; }
    }
}