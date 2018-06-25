using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using NotTetrin.Constants;
using NotTetrin.SceneManagement;

namespace NotTetrin.Ingame.Single.Stack {
    public class GameManager : MonoBehaviour {
        [SerializeField] private Director director;
        [SerializeField] private BGMManager bgmManager;
        [SerializeField] private IngameSfxManager sfxManager;
        [SerializeField] private MinoManager minoManager;
        [SerializeField] private Score score;
        [SerializeField] private HighScore highScore;
        [SerializeField] private Ranking ranking;
        
        private void Start() {
            minoManager.HitMino += onHitMino;

            ranking.gameObject.SetActive(true);
            loadRanking();
            gamestart();
        }

        private void Update() {
            if (Input.GetButtonDown(@"Escape")) {
                SceneTransit.Instance.LoadScene(SceneName.Title, 0.4f);
            }
        }

        private void reset() {
            CancelInvoke("gamestart");
            sfxManager.Stop(IngameSfxType.GameOver);
            score.Reset();
            minoManager.Reset();
        }

        private void gamestart() {
            reset();
            director.Floor.SetActive(true);
            bgmManager.RandomPlay();
            sfxManager.Play(IngameSfxType.GameStart);
            minoManager.Next();
        }

        private void gameover() {
            director.Floor.SetActive(false);
            bgmManager.Stop();
            sfxManager.Play(IngameSfxType.GameOver);

            var updated = highScore.UpdateValue();
            if (updated) {
                saveRanking();
            }
            Invoke("loadRanking", 3.0f);
            Invoke("gamestart", 9.0f);
        }

        private void loadRanking() {
            ranking.Fetch();
        }

        private void saveRanking() {
            var name = PlayerPrefs.GetString(PlayerPrefsKey.PlayerName);
            var score = highScore.Value;
            var ranker = new Ranker(name, score);
            ranking.Save(ranker);
        }

        private void onHitMino(object sender, EventArgs args) {
            // 天井に当たったらゲームオーバー
            if (director.Ceiling.IsHit) {
                minoManager.Release();
                gameover();
            } else {
                score.Increase(200);
                minoManager.Next();
            }
        }
    }
}