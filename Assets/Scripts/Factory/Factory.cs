using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class Factory : MonoBehaviour
    {
        [SerializeField] private EntityHolder _entityHolderPrefab;
        [SerializeField] private EntityClickable _entityClickablePrefab;

        [SerializeField] private EntityView _circleViewPrefab;
        [SerializeField] private CircleBody _circleBodyPrefab;

        [SerializeField] private EntityView _squareViewPrefab;
        [SerializeField] private SquareBody _squareBodyPrefab;

        public EntityHolder GetEntityHolder()
        {
            return Instantiate(_entityHolderPrefab);
        }

        public EntityClickable GetEntityClickable()
        {
            return Instantiate(_entityClickablePrefab);
        }

        #region Circle Block

        public EntityView GetCircleView()
        {
            return Instantiate(_circleViewPrefab);
        }

        public CircleBody GetCircleBody()
        {
            return Instantiate(_circleBodyPrefab);
        }

        #endregion


        #region Square Block

        public EntityView GetSquareView()
        {
            return Instantiate(_squareViewPrefab);
        }

        public SquareBody GetSquareBody()
        {
            return Instantiate(_squareBodyPrefab);
        }

        #endregion
    }
}
