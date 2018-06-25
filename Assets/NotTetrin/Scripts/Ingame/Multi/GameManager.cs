using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using NotTetrin.Constants;
using NotTetrin.SceneManagement;

namespace NotTetrin.Ingame.Multi {
    [RequireComponent(typeof(PhotonView))]
    public class GameManager : MonoBehaviour {
        [SerializeField] private Director director;
        [SerializeField] private BGMManager bgmManager;
        [SerializeField] private IngameAudioManager audioManager;
        [SerializeField] private MinoManager minoManager;

        public UnityEvent OnRoundStart;
        public UnityEvent OnRoundEnd;

        private PhotonView photonView;

        public PlayerSide PlayerSide => (PhotonNetwork.player.ID == 1) ? PlayerSide.Left : PlayerSide.Right;

        private double gameOverTime = 0.0;
        private bool accepted = false;
        private bool hitMino = false;
        private bool hitOpponent = false;

        private void Awake() {
            photonView = GetComponent<PhotonView>();
        }

        private void Start() {
            minoManager.HitMino += onHitMino;
            ready();
        }

        private void Update() {
            if (Input.GetButtonDown(@"Escape")) {
                if (PhotonNetwork.connected) { PhotonNetwork.Disconnect(); }
                SceneTransit.Instance.LoadScene(SceneName.Title, 0.4f);
            }
        }

        private void reset() {
            accepted = false;
            audioManager.Stop(IngameSfxType.GameOver);
            minoManager.Reset();
        }

        private void ready() {
            photonView.RPC(@"OnReadyOpponent", PhotonTargets.Others);
        }

        private void gamestart() {
            reset();
            OnRoundStart.Invoke();

            bgmManager.RandomPlay();
            audioManager.Play(IngameSfxType.GameStart);
            minoManager.Next();
        }

        private void gameover() {
            gameOverTime = PhotonNetwork.time;
            photonView.RPC(@"OnGameoverOpponent", PhotonTargets.Others, gameOverTime);
        }

        private void onHitMino(object sender, EventArgs args) {
            // 天井に当たったらゲームオーバー
            if (director.Ceiling.IsHit) {
                minoManager.Release();
                gameover();
            } else {
                /* ターン制 /
                photonView.RPC(@"OnOpponentHit", PhotonTargets.Others);
                if (hitOpponent) {
                    minoManager.Next();
                    hitOpponent = false;
                } else {
                    minoManager.Release();
                    hitMino = true;
                }
                /*/
                minoManager.Next();
                //*/
            }
        }

        public void OnPhotonPlayerDisconnected(PhotonPlayer player) {
            Debug.Log($"disconnected opponent.");
        }

        /* ターン制 /
        [PunRPC]
        private void OnOpponentHit() {
            if (hitMino) {
                minoManager.Next();
                hitMino = false;
            } else {
                hitOpponent = true;
            }
        }
        //*/

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
            audioManager.Play(IngameSfxType.GameOver);
            Invoke("ready", 9.0f);

            accepted = true;
        }

        [PunRPC]
        private void OnLoseAccepted() {
            if (accepted) { return; }
            Debug.Log($"you lose.");

            bgmManager.Stop();
            OnRoundEnd.Invoke();
            audioManager.Play(IngameSfxType.GameOver);
            Invoke("ready", 9.0f);

            accepted = true;
        }
    }
}
