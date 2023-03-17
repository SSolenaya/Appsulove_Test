using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace Assets.Scripts
{
    public class PlayerDataManager : IDisposable, IInitializable
    {
        [Inject] private Settings _settings;
        public ReactiveProperty<float> TotalDistance = new ReactiveProperty<float>();
        public ReactiveProperty<int> Score = new ReactiveProperty<int>();
        private PlayerData playerData;

        public void Initialize()
        {
            playerData = SaveManager.GetSavedData();
            TotalDistance.Value = playerData.totalDistance;
            Score.Value = playerData.totalScore;
            Score.Subscribe(_ => playerData.totalScore = _);
            TotalDistance.Subscribe(_ => playerData.totalDistance = _);
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
            SaveManager.Save(playerData);
            TotalDistance?.Dispose();
            Score?.Dispose();
        }

       
    }
}