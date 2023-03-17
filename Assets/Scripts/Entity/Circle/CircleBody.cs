using UnityEngine;

namespace Assets.Scripts
{
    public class CircleBody : PhysicalEntity, ICanTouch
    {
        public string GetName()
        {
            return gameObject.name;
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            ITouchable touchable = collider.gameObject.GetComponent<ITouchable>();
            touchable?.OnWasTouched(this);
        }
    }
}