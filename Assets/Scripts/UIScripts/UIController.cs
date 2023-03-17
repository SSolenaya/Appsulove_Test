using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace Assets.Scripts
{

    public class UIController : BaseUIPanel
    {
        [Inject] private PlayerDataManager _playerDataManager;

        [SerializeField] private TMP_Text _distanceText;
        [SerializeField] private TMP_Text _scoreText;

        private CompositeDisposable _disposable = new CompositeDisposable();

        private void Awake()
        {
            _playerDataManager.TotalDistance.Subscribe(OnDistanceChange).AddTo(_disposable);
            _playerDataManager.Score.Subscribe(OnScoreChange).AddTo(_disposable);

            ViewSetup();
        }

        private void OnDistanceChange(float newDist)
        {
            _distanceText.text = newDist.ToString("0.0");
        }

        private void OnScoreChange(int newScore)
        {
            _scoreText.text = newScore.ToString();
        }

        void OnDisable()
        {
            _disposable.Clear();
        }
    }
}
