using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(fileName = "Settings", menuName = "ScriptableObjects/Settings", order = 1)]
    public class Settings : ScriptableObject
    {
        public int SquaresStartAmount;
        public int SquaresMaxAmount;
        public int timeBeforeNewSquare;

        //  for positioning new squares outside existing objects on the field
        public int maximumEntitySize;         
        public int initialCircleSpeed;

        //  for right positioning squares inside field, equals size of square view
        public int fieldOffset;

        public int scoreForOneSquare;
    }
}


