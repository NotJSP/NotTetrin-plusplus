using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotTetrin.Ingame.MultiPlay {
    public class WinsCounter : MonoBehaviour {
        [SerializeField] SpriteRenderer[] stars;

        private Color disableColor = new Color(0.8f, 0.8f, 0.8f);
        private Color enableColor = new Color(1.0f, 1.0f, 0.0f);

        private int count;
        public bool CountLimited => (count >= stars.Length);

        private void Awake() {
            updateStars();
        }

        public void Increment() {
            count++;
            updateStars();
        }

        private void updateStars() {
            for (int i = 0; i < stars.Length; i++) {
                var color = (i < count) ? enableColor : disableColor;
                stars[i].color = color;
            }
        }
    }
}