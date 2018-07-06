using UnityEngine;
using NotTetrin.Ingame.Marathon;

namespace NotTetrin.Ingame {
    public abstract class Director : MonoBehaviour {
        public abstract GameObject Floor { get; }
        public abstract Ceiling Ceiling { get; }
        public abstract HoldMino HoldMino { get; }
        public abstract NextMino NextMino { get; }
        public abstract CollidersField CollidersField { get; }
    }
}
