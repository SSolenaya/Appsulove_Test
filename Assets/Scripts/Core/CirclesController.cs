using UnityEngine;
using Zenject;

namespace Assets.Scripts
{
    public class CirclesController
    {
        [Inject] private EngineManager _engineManager;
        [Inject] private Factory _factory;
        [Inject] private MainCanvas _mainCanvas;
        [Inject] private Field _field;
        [Inject] private PlayerDataManager _playerDataManager;
        [Inject] private Settings _settings;

        private CircleEntity _currentCircleEntity;

        [Inject]
        private void Setup()
        {
            CreateCircleEntity();
        }

        private void CreateCircleEntity()
        {
            _currentCircleEntity = new CircleEntity(_factory, _mainCanvas, _engineManager, _field, _field, _playerDataManager, _settings);
        }

        public Vector3 GetPositionCircleEntity()
        {
            return _currentCircleEntity.GetPosition();
        }
    }
}