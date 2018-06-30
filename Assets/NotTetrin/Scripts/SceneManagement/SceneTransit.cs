using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

namespace NotTetrin.SceneManagement {
    public class SceneTransit : SingletonMonoBehaviour<SceneTransit> {
        private bool fading = false;
        private float fadeAlpha = 0.0f;
        public Color fadeColor = Color.black;

        private Rect rect => new Rect(0, 0, Screen.width, Screen.height);

        public void OnGUI() {
            if (fadeAlpha > 0) {
                fadeColor.a = fadeAlpha;
                GUI.color = fadeColor;
                GUI.DrawTexture(rect, Texture2D.whiteTexture);
            }
        }

        public void LoadScene(string scene, float interval) {
            StartCoroutine(transitScene(scene, interval));
        }

        private IEnumerator transitScene(string scene, float interval) {
            // シーン読み込み
            var operation = SceneManager.LoadSceneAsync(scene);
            operation.allowSceneActivation = false;

            // フェードイン
            StartCoroutine(fadeIn(interval));
            yield return new WaitForSeconds(interval);

            // ロードアニメーション
            //animation.Play(operation);
            yield return new WaitWhile(() => operation.progress < 0.9f);

            // シーンをアクティブ化
            //animation.Stop();
            operation.allowSceneActivation = true;
            yield return new WaitUntil(() => operation.isDone);

            // フェードアウト
            StartCoroutine(fadeOut(interval));
        }

        private IEnumerator fadeIn(float interval) {
            fading = true;

            var time = 0.0f;
            while (time <= interval) {
                fadeAlpha = Mathf.Lerp(0f, 1f, time / interval);
                time += Time.deltaTime;
                yield return null;
            }

            fading = false;
        }

        private IEnumerator fadeOut(float interval) {
            fading = true;

            var time = 0.0f;
            while (time <= interval) {
                fadeAlpha = Mathf.Lerp(1f, 0f, time / interval);
                time += Time.deltaTime;
                yield return null;
            }

            fading = false;
        }
    }
}