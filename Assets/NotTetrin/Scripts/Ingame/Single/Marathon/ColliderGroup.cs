using System.Linq;
using System.Collections.Generic;

namespace NotTetrin.Ingame.Single.Marathon {
    public class ColliderGroup {
        public IEnumerable<ColliderHelper> Children { get; private set; }

        public ColliderGroup(IEnumerable<ColliderHelper> colliders) {
            this.Children = colliders;
        }

        public bool EnteredAll => Children.All(c => c.IsEntered);
        public int EnterCount => Children.Count(c => c.IsEntered);
    }
}