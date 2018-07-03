using UnityEngine;
using NotTetrin.Ingame.MultiPlay.Marathon;

namespace NotTetrin.Ingame.MultiPlay {
    public class NetworkDirector : Director {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private GameObject[] floors;
        [SerializeField] private Ceiling[] ceilings;
        [SerializeField] private HoldMino[] holdMinos;
        [SerializeField] private NextMino[] nextMinos;
        [SerializeField] private GameObject[] collidersFields;

        private int sideIndex => (gameManager.PlayerSide == PlayerSide.Left) ? 0 : 1;

        public override GameObject Floor => floors[sideIndex];
        public override Ceiling Ceiling => ceilings[sideIndex];
        public override HoldMino HoldMino => holdMinos[sideIndex];
        public override NextMino NextMino => nextMinos[sideIndex];
        public override GameObject CollidersField => collidersFields[sideIndex];
    }
}
