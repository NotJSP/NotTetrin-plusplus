using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using NotTetrin.Ingame.Marathon;

namespace NotTetrin.Ingame.SinglePlay.Marathon {
    public class LineDeleteScoring : MonoBehaviour {
        [SerializeField] private GroupManager groupManager;
        [SerializeField] private Score score;

        private void Awake() {
            groupManager.MinoDeleted += onMinoDeleted;
        }

        private void onMinoDeleted(object sender, DeleteMinoInfo info) {
            var baseScore = 500.0;
            var lines = info.LineCount;
            var amount = baseScore * Math.Pow(lines, 2) - baseScore * 0.15 * Math.Pow(lines, 2);
            score.Increase((int)amount);
        }
    }
}
