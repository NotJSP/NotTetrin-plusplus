using System.Linq;

namespace NotTetrin.Ingame.Single.Marathon {
    public class ColliderGroup {
        public ColliderHelper[] Children { get; private set; }

        public ColliderGroup(ColliderHelper[] colliders) {
            this.Children = colliders;
        }

        public bool EnteredAll => Children.All(c => c.IsEntered);
        public int EnterCount => Children.Count(c => c.IsEntered);
    }
}