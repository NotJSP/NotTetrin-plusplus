using UnityEngine;
using UnityEngine.UI;
using NotTetrin.Ingame.MultiPlay.Marathon;
using NotTetrin.Ingame.Marathon;

namespace NotTetrin.Ingame.MultiPlay {
    public class NetworkDirector : Director {
        [SerializeField] GameManager gameManager;

        [SerializeField] GameObject[] floors;
        [SerializeField] Ceiling[] ceilings;
        [SerializeField] HoldMino[] holdMinos;
        [SerializeField] NextMino[] nextMinos;

        [SerializeField] Text[] pingLabels;
        [SerializeField] Text[] youLabels;
        [SerializeField] Text[] nameLabels;
        [SerializeField] WinsCounter[] winsCounters;

        private int playerIndex => (gameManager.PlayerSide == PlayerSide.Left) ? 0 : 1;
        private int opponentIndex => (gameManager.PlayerSide == PlayerSide.Left) ? 1 : 0;

        public override GameObject Floor => floors[playerIndex];
        public override Ceiling Ceiling => ceilings[playerIndex];
        public override HoldMino HoldMino => holdMinos[playerIndex];
        public override NextMino NextMino => nextMinos[playerIndex];

        public Text PlayerPingLabel => pingLabels[playerIndex];
        public Text PlayerYouLabel => youLabels[playerIndex];
        public Text PlayerNameLabel => nameLabels[playerIndex];
        public WinsCounter PlayerWinsCounter => winsCounters[playerIndex];
        public Text OpponentPingLabel => pingLabels[opponentIndex];
        public Text OpponentYouLabel => youLabels[playerIndex];
        public Text OpponentNameLabel => nameLabels[opponentIndex];
        public WinsCounter OpponentWinsCounter => winsCounters[opponentIndex];
    }
}
