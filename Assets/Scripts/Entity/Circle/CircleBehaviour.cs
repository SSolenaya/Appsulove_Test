using System;
using System.Collections.Generic;
using UniRx;
using UniRx.Async;
using UnityEngine;

namespace Assets.Scripts
{
    public class CircleBehaviour : IDisposable
    {
        private readonly EntityHolder _entityHolder;
        private readonly MovementLogic _movementLogic;
        private readonly EntityClickable _entityClickable;
        private readonly PlayerDataManager _playerDataManager;
        private readonly IReturnClickPosition _returnClickPosition;
        private readonly IReturnDragPosition _returnDragPosition;
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        private readonly Queue<Vector3> _queuePathPoint = new Queue<Vector3>();
        private bool _isDragging;

        public CircleBehaviour(EntityHolder entityHolder,
            MovementLogic movementLogic,
            EntityClickable entityClickable,
            PlayerDataManager playerDataManager,
            IReturnClickPosition returnClickPosition,
            IReturnDragPosition returnDragPosition)
        {
            _entityHolder = entityHolder;
            _movementLogic = movementLogic;
            _entityClickable = entityClickable;
            _playerDataManager = playerDataManager;
            _returnClickPosition = returnClickPosition;
            _returnDragPosition = returnDragPosition;
            CreateBehaviour();
        }

        private void CreateBehaviour()
        {   
            //  subscribe view for calculating position by movement logic
            _movementLogic.CurrentPosition.Subscribe(_entityHolder.SetPosition).AddTo(_disposable);


            //  subscribe counting distance for current distance by movement logic
            _movementLogic.DeltaDistance.Subscribe(_playerDataManager.IncreaseDistance).AddTo(_disposable);

            //  subscribe move movementLogic for click on field
            _returnClickPosition.ActionClickPosition += var => {
                if (_isDragging)
                {
                    return;
                }
                ClearPath();
                _movementLogic.MoveTo(var);
            };

            //  subscribe for drag logic
            _returnDragPosition.ActionOnBeginDragPosition += OnBeginDrag;
            _returnDragPosition.ActionOnDragPosition += OnDrag;
            _returnDragPosition.ActionOnEndDragPosition += OnEndDrag;

            //  subscribe stop of movement for click on entity
            _entityClickable.CommandOnClick.Subscribe(_ => {
                ClearPath();
                _movementLogic.StopMove();
            }).AddTo(_disposable);

            //  circle's starting position at the center of the screen
            _movementLogic.FastMoveTo(Vector3.zero); 
        }

        private void OnBeginDrag(Vector3 vector3)
        {
            _isDragging = true;
            ClearPath();
            _queuePathPoint.Enqueue(vector3);
        }

        private void OnDrag(Vector3 vector3)
        {
            _queuePathPoint.Enqueue(vector3);
        }

        private void OnEndDrag(Vector3 vector3)
        {
            _isDragging = false;
            _queuePathPoint.Enqueue(vector3);
            PathFingerMove(_queuePathPoint.Dequeue());
        }

        private async void PathFingerMove(Vector3 v3)
        {
            _movementLogic.MoveTo(v3);

            await UniTask.WaitUntil(() => !_movementLogic.IMove);
            if (_queuePathPoint.Count > 0)
            {
                PathFingerMove(_queuePathPoint.Dequeue());
            }
        }

        private void ClearPath()
        {
            _queuePathPoint.Clear();
        }

        public void Dispose()
        {
            _returnClickPosition.ActionClickPosition -= _movementLogic.MoveTo;
            _disposable.Clear();
        }
    }
}