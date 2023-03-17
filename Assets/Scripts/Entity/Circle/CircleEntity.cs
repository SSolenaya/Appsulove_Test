using UnityEngine;
using Zenject;

namespace Assets.Scripts
{
    public class CircleEntity
    {
        private EntityView _circleView;
        private CircleBody _circleBody;
        private EntityHolder _entityHolder;
        private MovementLogic _movementLogic;
        private EntityClickable _entityClickable;
        private CircleBehaviour _circleBehaviour;

        public CircleEntity(Factory factory, 
            MainCanvas mainCanvas, 
            EngineManager engineManager, 
            IReturnClickPosition returnClickPosition, 
            IReturnDragPosition returnDragPosition, 
            PlayerDataManager playerDataManager, 
            Settings settings)
        {
            _movementLogic = new MovementLogic(engineManager, settings);

            _entityHolder = factory.GetEntityHolder();
            _entityHolder.Setup(mainCanvas.CanvasParent, "CircleEntity");

            _circleView = factory.GetCircleView();
            _circleView.Setup(_entityHolder);

            _circleBody = factory.GetCircleBody();
            _circleBody.Setup(_entityHolder);

            _entityClickable = factory.GetEntityClickable();
            _entityClickable.Setup(_entityHolder);

            _circleBehaviour = new CircleBehaviour(_entityHolder, _movementLogic, _entityClickable, playerDataManager, returnClickPosition, returnDragPosition);
        }

        public Vector3 GetPosition()
        {
            return _entityHolder.RectTransform.position;
        }
    }
}