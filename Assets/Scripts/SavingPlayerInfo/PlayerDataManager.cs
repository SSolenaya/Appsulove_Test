using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace Assets.Scripts
{
    public class PlayerDataManager : IDisposable
    {
        [Inject] private EngineManager _engineManager;
        [Inject] private Settings _settings;
        public ReactiveProperty<float> TotalDistance = new ReactiveProperty<float>();
        public ReactiveProperty<int> Score = new ReactiveProperty<int>();
        private CompositeDisposable _disposable = new CompositeDisposable();
        private PlayerData playerData;

        [Inject]
        private void Setup()
        {
            playerData = SaveManager.GetSavedData();
            TotalDistance.Value = playerData.totalDistance;
            Score.Value = playerData.totalScore;
            Score.Subscribe(_ => playerData.totalScore = _);
            TotalDistance.Subscribe(_ => playerData.totalDistance = _);
            _engineManager.OnApplicationQuitCommand.Subscribe(_ => SaveManager.Save(playerData)).AddTo(_disposable);
        }

        public void IncreaseDistance(float deltaDistance)
        {
            TotalDistance.Value += deltaDistance;
        }

        public void IncreaseScore()
        {
            Score.Value += _settings.scoreForOneSquare;
        }

        public void Dispose()
        {
            TotalDistance?.Dispose();
            Score?.Dispose();
            _disposable?.Dispose();
        }
    }
}