using UnityEngine;
using NotTetrin.Ingame.Marathon;

namespace NotTetrin.Ingame.MultiPlay.Marathon {
    public class NetworkMarathonDirector : MarathonDirector {
        [SerializeField] GameManager gameManager;

        [SerializeField] GameObject[] sideWalls;
        [SerializeField] CollidersField[] collidersFields;

        private int playerIndex => (gameManager.PlayerSide == PlayerSide.Left) ? 0 : 1;
        private int opponentIndex => (gameManager.PlayerSide == PlayerSide.Left) ? 1 : 0;

        public override GameObject SideWall => sideWalls[playerIndex];
        public override CollidersField CollidersField => collidersFields[playerIndex];
    }
}
