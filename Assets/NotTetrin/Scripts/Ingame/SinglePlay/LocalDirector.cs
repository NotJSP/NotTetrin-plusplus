using UnityEngine;
using NotTetrin.Ingame.Marathon;

namespace NotTetrin.Ingame.SinglePlay {
    public class LocalDirector : Director {
        [SerializeField] private GameObject floor;
        [SerializeField] private Ceiling ceiling;
        [SerializeField] private HoldMino holdMino;
        [SerializeField] private NextMino nextMino;
        [SerializeField] private CollidersField collidersField;

        public override GameObject Floor => floor;
        public override Ceiling Ceiling => ceiling;
        public override HoldMino HoldMino => holdMino;
        public override NextMino NextMino => nextMino;
        public override CollidersField CollidersField => collidersField;
    }
}
