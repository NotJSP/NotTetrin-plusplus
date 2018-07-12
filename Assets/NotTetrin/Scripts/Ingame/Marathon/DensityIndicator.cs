﻿using UnityEngine;
using NotTetrin.Utility;

namespace NotTetrin.Ingame.Marathon {
    [RequireComponent(typeof(SpriteRenderer))]
    public class DensityIndicator : MonoBehaviour {
        private new SpriteRenderer renderer;
        private float maxScaleX;
        private float halfSizeX;
        private Vector3 defaultPosition;
        private Vector3 scale;
        private Color color;

        public void Initialize(GameObject wall) {
            renderer = GetComponent<SpriteRenderer>();

            var rateX = wall.size().x / gameObject.size().x;
            maxScaleX = transform.localScale.x * rateX;

            scale = new Vector3(maxScaleX, transform.localScale.y);
            transform.localScale = scale;
            halfSizeX = 0.5f * gameObject.size().x;

            scale.x = 0.0f;
            transform.localScale = scale;

            defaultPosition = new Vector3(wall.transform.position.x - halfSizeX, transform.position.y);
            color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 0.0f);

            transform.position = defaultPosition;
            renderer.color = color;
        }

        public void UpdateDensity(float density) {
            var density_pow = Mathf.Pow(density, 2.0f);

            var position = new Vector3(defaultPosition.x + density_pow * halfSizeX, defaultPosition.y);
            transform.position = position;

            scale.x = maxScaleX * density_pow;
            transform.localScale = scale;

            color.a = Mathf.Pow(density_pow, 2.5f);
            renderer.color = color;
        }
    }
}
