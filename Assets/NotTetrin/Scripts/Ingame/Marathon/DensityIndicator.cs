using UnityEngine;
using NotTetrin.Utility;

namespace NotTetrin.Ingame.Marathon {
    [RequireComponent(typeof(SpriteRenderer))]
    public class DensityIndicator : MonoBehaviour {
        private new SpriteRenderer renderer;
        private Vector3 localScale;
        private float maxScaleX;

        public void Initialize(GameObject wall) {
            this.localScale = transform.localScale;
            this.renderer = GetComponent<SpriteRenderer>();

            var rateX = wall.size().x / gameObject.size().x;
            maxScaleX = localScale.x * rateX;

            transform.position = new Vector3(wall.transform.position.x, transform.position.y);
            transform.localScale = new Vector3(0, localScale.y, localScale.z);
        }

        public void UpdateDensity(float density) {
            var density_pow = Mathf.Pow(density, 2.5f);
            transform.localScale = new Vector3(maxScaleX * density_pow, localScale.y, localScale.z);

            var color = renderer.color;
            color.a = density_pow;
            renderer.color = color;
        }
    }
}
