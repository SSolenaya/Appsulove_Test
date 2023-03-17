using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace Assets.Scripts
{
    public class MovementLogic : IDisposable
    {
        private Settings _settings;

        public ReactiveProperty<Vector3> CurrentPosition = new ReactiveProperty<Vector3>();
        public ReactiveProperty<float> DeltaDistance = new ReactiveProperty<float>();

        private Vector3 _targetPosition;
        private  float _currentSpeed;
        private readonly EngineManager _engineManager;
        private CompositeDisposable _disposable = new CompositeDisposable();

        public bool IMove;

        public MovementLogic(EngineManager engineManager, Settings settings)
        {
            _settings = settings;
            _engineManager = engineManager;
            _engineManager.OnFixedUpdateCommand.Subscribe(UpdateMovement).AddTo(_disposable);
            _currentSpeed = _settings.initialCircleSpeed;
        }

        public void StopMove()
        {
            _targetPosition = CurrentPosition.Value;
            IMove = false;
        }

        public void FastMoveTo(Vector3 position)
        {
            CurrentPosition.Value = position;
            _targetPosition = CurrentPosition.Value;
        }
        
        public void MoveTo(Vector3 position)
        {
            _targetPosition = position;
        }

        private void UpdateMovement(float fixedDeltaTime)
        {
            if (CurrentPosition.Value == _targetPosition)
            {
                IMove = false;
                return;
            }

            IMove = true;
            float distance = _currentSpeed * fixedDeltaTime;
            DeltaDistance.SetValueAndForceNotify(distance);
            CurrentPosition.Value = Vector3.MoveTowards(CurrentPosition.Value, _targetPosition, distance);
        }


        public void Dispose()
        {
            CurrentPosition?.Dispose();
            DeltaDistance?.Dispose();
            _disposable?.Dispose();
        }
    }
}