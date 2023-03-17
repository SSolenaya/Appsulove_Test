using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Assets.Scripts;
using Assets.Scripts.Utils;
using UniRx;
using UnityEngine;
using Zenject;

namespace Assets.Scripts
{
    public class SquaresController : IInitializable, IDisposable
    {
        [Inject] private Factory _factory;
        [Inject] private Field _field;
        [Inject] private MainCanvas _mainCanvas;
        [Inject] private Settings _settings;
        [Inject] private PlayerDataManager _playerDataManager;
        [Inject] private CirclesController _circlesController;
        [Inject] private CountdownTimer _createSquareTimer;
        [SerializeField] private SquareEntity _squareEntityPrefab;
        private List<SquareEntity> listSquareEntity = new List<SquareEntity>();
        private CompositeDisposable _disposable = new CompositeDisposable();


        public void Initialize()
        {
            CreateOnStart();
            CreateSquareTimer();
        }
        

        private void CreateOnStart()
        {
            CreateSquares(_settings.SquaresStartAmount);
        }

        private void CreateSquares(int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (listSquareEntity.Count < _settings.SquaresMaxAmount)
                {
                    CreateSquare();
                }
            }
        }

        private void CreateSquareTimer()
        {
            _createSquareTimer.Setup(_settings.timeBeforeNewSquare, () => {
                CreateSquares(1);
                CreateSquareTimer();
            });
        }

        private void CreateSquare()
        {
            if (_field.GetRandomPosition(GetExceptPoints(), out var startPos))
            {
                SquareEntity square = new SquareEntity(startPos, _factory, _mainCanvas);
                square.SubscribeForDestroyCommand(HideSquare);
                listSquareEntity.Add(square);
            }
        }

        public void HideSquare(SquareEntity squareToDelete)
        {
            listSquareEntity.Remove(squareToDelete);
            _playerDataManager.IncreaseScore();
        }

        private List<Vector3> GetExceptPoints()
        {
            List<Vector3> exceptPoints = new List<Vector3>();
            foreach (SquareEntity squareEntity in listSquareEntity)
            {
                exceptPoints.Add(squareEntity.GetPosition());
            }

            exceptPoints.Add(_circlesController.GetPositionCircleEntity());
            return exceptPoints;
        }

        public void Dispose()
        {
            _disposable.Clear();
        }

        
    }
}