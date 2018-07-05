using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using NotTetrin.Constants;
using NotTetrin.SceneManagement;
using NotTetrin.Ingame.Marathon;

namespace NotTetrin.Ingame.MultiPlay.Marathon {
    [RequireComponent(typeof(PhotonView))]
    public class GameManager : SceneBase {
        [SerializeField] private Director director;
        [SerializeField] private BGMManager bgmManager;
        [SerializeField] private GroupManager groupManager;
        [SerializeField] private IngameSfxManager sfxManager;
        [SerializeField] private MinoManager minoManager;
        [SerializeField] private GarbageMinoManager garbageMinoManager;

        private PhotonView photonView;
        private double gameOverTime = 0.0;
        private bool accepted = false;

        public PlayerSide PlayerSide => (PhotonNetwork.player.ID == 1) ? PlayerSide.Left : PlayerSide.Right;

        protected override void Awake() {
            base.Awake();
            photonView = GetComponent<PhotonView>();
            groupManager.MinoDeleted += onMinoDeleted;

            foreach (var clip in bgmManager.Clips) {
                bgmManager.Add(clip);
            }
        }

        protected override void OnSceneReady(object sender, EventArgs args) {
            base.OnSceneReady(sender, args);
            minoManager.HitMino += onHitMino;
            ready();
        }

        private void Update() {
            if (Input.GetButtonDown(@"Escape")) {
                if (PhotonNetwork.connected) { PhotonNetwork.Disconnect(); }
                SceneController.Instance.LoadScene(SceneName.Title, 0.7f);
            }
        }

        private void reset() {
            accepted = false;
            sfxManager.Stop(IngameSfxType.GameOver);
            minoManager.Reset();
            garbageMinoManager.Clear();
        }

        private void ready() {
            photonView.RPC(@"OnReadyOpponent", PhotonTargets.Others);
        }

        private void gamestart() {
            reset();
            director.Floor.SetActive(true);
            bgmManager.RandomPlay();
            sfxManager.Play(IngameSfxType.GameStart);
            minoManager.Next();
        }

        private void gameover() {
            gameOverTime = PhotonNetwork.time;
            photonView.RPC(@"OnGameoverOpponent", PhotonTargets.Others, gameOverTime);
        }

        private void next() {
            minoManager.Next();
        }

        private void onMinoDeleted(object sender, DeleteMinoInfo info) {
            photonView.RPC(@"OnDeleteMinoOpponent", PhotonTargets.Others, info.LineCount, info.ObjectCount);
        }

        private void onHitMino(object sender, EventArgs args) {
            minoManager.Release();

            // 天井に当たったらゲームオーバー
            if (director.Ceiling.IsHit) {
                gameover();
            } else {
                groupManager.DeleteMino();
                StartCoroutine(fallGarbageAndNext());
            }
        }

        private IEnumerator fallGarbageAndNext() {
            if (garbageMinoManager.Fall()) {
                yield return new WaitWhile(() => garbageMinoManager.IsFalling);
            }
            next();
        }

        public void OnPhotonPlayerDisconnected(PhotonPlayer player) {
            Debug.Log($"disconnected opponent.");
        }

        [PunRPC]
        private void OnReadyOpponent() {
            gamestart();
        }

        [PunRPC]
        private void OnGameoverOpponent(double timestamp) {
            Debug.Log(@"OnGameoverOpponent (" + timestamp + ")");
            minoManager.Destroy();

            if (timestamp < gameOverTime) {
                photonView.RPC(@"OnWinAccepted", PhotonTargets.Others);
                OnLoseAccepted();
            } else {
                photonView.RPC(@"OnLoseAccepted", PhotonTargets.Others);
                OnWinAccepted();
            }
        }

        [PunRPC]
        private void OnWinAccepted() {
            if (accepted) { return; }
            Debug.Log($"you win.");

            bgmManager.Stop();
            sfxManager.Play(IngameSfxType.GameOver);
            Invoke("ready", 9.0f);

            accepted = true;
        }

        [PunRPC]
        private void OnLoseAccepted() {
            if (accepted) { return; }
            Debug.Log($"you lose.");

            bgmManager.Stop();
            director.Floor.SetActive(false);
            sfxManager.Play(IngameSfxType.GameOver);
            Invoke("ready", 9.0f);

            accepted = true;
        }

        [PunRPC]
        private void OnDeleteMinoOpponent(int lineCount, int objectCount) {
            var info = new DeleteMinoInfo(lineCount, objectCount);
            garbageMinoManager.Add(info);
        }
    }
}
