using UnityEngine;

namespace NotTetrin.Ingame.Multi {
    public class NetworkDirector : Director {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private GameObject[] floors;
        [SerializeField] private Ceiling[] ceilings;
        [SerializeField] private HoldMino[] holdMinos;
        [SerializeField] private NextMino[] nextMinos;

        private int sideIndex => (gameManager.PlayerSide == PlayerSide.Left) ? 0 : 1;

        public override GameObject Floor => floors[sideIndex];
        public override Ceiling Ceiling => ceilings[sideIndex];
        public override HoldMino HoldMino => holdMinos[sideIndex];
        public override NextMino NextMino => nextMinos[sideIndex];
    }
}
