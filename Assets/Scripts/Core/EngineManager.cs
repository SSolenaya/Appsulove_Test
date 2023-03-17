using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UniRx;
using UnityEngine;

namespace Assets.Scripts
{
    public class EngineManager : MonoBehaviour
    {
        public ReactiveCommand OnApplicationQuitCommand = new ReactiveCommand();
        public ReactiveCommand<float> OnUpdateCommand = new ReactiveCommand<float>();
        public ReactiveCommand<float> OnFixedUpdateCommand = new ReactiveCommand<float>();

        void Update()
        {
            OnUpdateCommand?.Execute(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            OnFixedUpdateCommand?.Execute(Time.fixedDeltaTime);
        }

        private void OnApplicationQuit()
        {
            OnApplicationQuitCommand?.Execute();
        }
    }
}
