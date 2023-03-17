using UnityEngine;

namespace Assets.Scripts {
    public interface IClickReactor
    {
        public void ReactionOnClick(Vector3 clickPosition);
    }
}
