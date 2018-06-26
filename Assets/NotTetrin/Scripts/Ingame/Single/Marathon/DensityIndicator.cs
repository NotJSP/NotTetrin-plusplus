using UnityEngine;

namespace NotTetrin.Ingame.Single.Marathon {
    public class DensityIndicator : MonoBehaviour {
        private GameObject obj;

        private Vector3 localScale => obj.transform.localScale;

        public void UpdateDensity(float density) {
            obj.transform.localScale = new Vector3(density, localScale.y);
        }
    }
}
