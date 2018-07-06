using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using NotTetrin.Utility.Tiling;

namespace NotTetrin.Ingame.Marathon {
    [RequireComponent(typeof(TileCreator))]
    public class CollidersField : MonoBehaviour {
        private TileCreator creator => GetComponent<TileCreator>();
        public GameObject[] Create() => creator.Create();
    }
}
