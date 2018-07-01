using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

namespace NotTetrin.SceneManagement {
    public class SceneController : SingletonMonoBehaviour<SceneController> {
        [SerializeField] private Canvas canvas;
        [SerializeField] private LoadingScreen screen;

        public event EventHandler SceneReady;
        private Coroutine coroutine;

        private void Start() {
            SceneReady?.Invoke(this, EventArgs.Empty);
        }

        public void LoadScene(string scene, float interval) {
            if (coroutine != null) { return; }
            coroutine = StartCoroutine(transitScene(scene, interval));
        }

        private IEnumerator transitScene(string scene, float interval) {
            // シーン読み込み
            var operation = SceneManager.LoadSceneAsync(scene);
            operation.allowSceneActivation = false;

            // フェードイン
            canvas.enabled = true;
            screen.FadeIn(interval);
            // ロードアニメーション
            screen.Play(operation);
            yield return new WaitWhile(() => screen.isFading);
            yield return new WaitWhile(() => operation.progress < 0.9f);

            // シーンをアクティブ化
            operation.allowSceneActivation = true;
            yield return new WaitUntil(() => operation.isDone);

            // シーン準備完了
            SceneReady?.Invoke(this, EventArgs.Empty);

            // フェードアウト
            screen.FadeOut(interval);
            yield return new WaitWhile(() => screen.isFading);
            screen.Stop();
            canvas.enabled = false;

            // コルーチンの削除
            coroutine = null;
        }
    }
}