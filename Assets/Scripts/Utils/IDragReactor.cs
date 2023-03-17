using UnityEngine;

namespace Assets.Scripts {
    public interface IDragReactor
    {
        public void ReactionOnBeginDrag(Vector3 clickPosition);
        public void ReactionOnDrag(Vector3 clickPosition);
        public void ReactionOnEndDrag(Vector3 clickPosition);
    }
}
