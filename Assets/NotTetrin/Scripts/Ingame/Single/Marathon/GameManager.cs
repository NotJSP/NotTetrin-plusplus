using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using NotTetrin.Constants;
using NotTetrin.SceneManagement;

using Random = UnityEngine.Random;

namespace NotTetrin.Ingame.Single.Marathon {
    public class GameManager : MonoBehaviour {
        [SerializeField] private Director director;
        [SerializeField] private IngameAudioManager audioManager;
        [SerializeField] private MinoManager minoManager;
        [SerializeField] private Score score;
        [SerializeField] private HighScore highScore;
        [SerializeField] private Ranking ranking;
        [SerializeField] private GroupManager groupManager;

        [SerializeField] private AudioClip[] bgmClips;

        public UnityEvent OnRoundStart;
        public UnityEvent OnRoundEnd;

        private AudioSource bgmAudioSource;

        private void Awake() {
            bgmAudioSource = GetComponent<AudioSource>();
        }

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
            audioManager.Stop(IngameSfxType.GameOver);
            score.Reset();
            minoManager.Reset();
        }

        private void gamestart() {
            reset();
            OnRoundStart.Invoke();

            var clipIndex = Random.Range(0, bgmClips.Length - 1);
            bgmAudioSource.clip = bgmClips[clipIndex];
            bgmAudioSource.Play();

            audioManager.Play(IngameSfxType.GameStart);
            minoManager.Next();
        }

        private void gameover() {
            OnRoundEnd.Invoke();
            bgmAudioSource.Stop();
            audioManager.Play(IngameSfxType.GameOver);

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
                groupManager.DeleteMino();
                minoManager.Next();
            }
        }
    }
}