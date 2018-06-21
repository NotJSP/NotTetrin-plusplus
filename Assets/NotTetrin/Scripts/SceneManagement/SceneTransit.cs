using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

namespace NotTetrin.SceneManagement {
    public class SceneTransit : SingletonMonoBehaviour<SceneTransit> {
        private float fadeAlpha = 0;
        private bool isFading = false;
        public Color fadeColor = Color.black;

        public void Awake() {
            if (this != Instance) {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
        }

        public void OnGUI() {
            if (isFading) {
                fadeColor.a = fadeAlpha;
                GUI.color = fadeColor;
                GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
            }
        }

        public void LoadScene(string scene, float interval) {
            StartCoroutine(transitScene(scene, interval));
        }

        private IEnumerator transitScene(string scene, float interval) {
            isFading = true;
            float time = 0;
            while (time <= interval) {
                fadeAlpha = Mathf.Lerp(0f, 1f, time / interval);
                time += Time.deltaTime;
                yield return 0;
            }

            SceneManager.LoadScene(scene);

            time = 0;
            while (time <= interval) {
                fadeAlpha = Mathf.Lerp(1f, 0f, time / interval);
                time += Time.deltaTime;
                yield return 0;
            }

            isFading = false;
        }
    }
}