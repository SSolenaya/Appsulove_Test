using System;
using UniRx;
using UnityEngine;

namespace Assets.Scripts
{
    public class SquareEntity
    {
        private readonly Factory _factory;
        private readonly MainCanvas _mainCanvas;
        private readonly EntityView _squareView;
        private readonly SquareBody _squareBody;
        private readonly EntityHolder _entityHolder;

        private readonly ReactiveCommand<SquareEntity> DestroyCommand = new ReactiveCommand<SquareEntity>();
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public SquareEntity(Vector3 startPosition, Factory factory, MainCanvas mainCanvas)
        {
            _factory = factory;
            _mainCanvas = mainCanvas;

            _entityHolder = factory.GetEntityHolder();
            _entityHolder.Setup(_mainCanvas.CanvasParent, "SquareEntity");
            _entityHolder.SetPosition(startPosition);

            _squareView = _factory.GetSquareView();
            _squareView.Setup(_entityHolder);

            _squareBody = _factory.GetSquareBody();
            _squareBody.Setup(_entityHolder, DestroySquare);
        }

        public void SubscribeForDestroyCommand(Action<SquareEntity> act)
        {
            DestroyCommand.Subscribe(_ => act?.Invoke(this)).AddTo(_disposable);
        }

        public Vector3 GetPosition()
        {
            return _entityHolder.RectTransform.anchoredPosition3D;
        }

        private void DestroySquare()
        {
            DestroyCommand.Execute(this);
            _disposable.Clear();
            GameObject.Destroy(_entityHolder.gameObject);
        }
    }
}