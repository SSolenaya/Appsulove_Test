using System;
using UniRx;
using UnityEngine;

namespace Assets.Scripts
{
    public class SquareBody : PhysicalEntity, ITouchable
    {
        private ReactiveCommand WasTouchedCommand = new ReactiveCommand();

        public void Setup(EntityHolder entityHolder, Action actionWasTouched)
        {
            Setup(entityHolder);
            WasTouchedCommand.Subscribe(_ => actionWasTouched.Invoke());
        }
        
        public void OnWasTouched(ICanTouch canTouch)
        {
            WasTouchedCommand.Execute();
        }
    }
}