﻿using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NCMB;
using NotTetrin.Constants;
using NotTetrin.Utility;

namespace NotTetrin.Ingame.SinglePlay {
    public class Ranking : MonoBehaviour {
        private Coroutine fetchCoroutine = null;
        private Coroutine saveCoroutine = null;

        private static readonly int FetchCount = 10;

        [SerializeField]
        private Text textField;
        [SerializeField]
        private HighScore highScore;

        private ASyncValue<int, NCMBException> currentRank = new ASyncValue<int, NCMBException>();
        private ASyncValue<List<Ranker>, NCMBException> topRankers = new ASyncValue<List<Ranker>, NCMBException>();

        private StringBuilder builder;

        private string getClassName(RankingType type) {
            switch (type) {
                case RankingType.StackMode:
                    return @"StackRanking";
                case RankingType.MarathonMode:
                    // TODO: 本来「MarathonRanking」を使用する予定だが、現在のランキングの都合上そのまま使用している（NCMB側でクラス名が変更できない、コピーもできない)
                    return @"Ranking";
                    // return @"MarathonRanking";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Fetch(RankingType type) {
            if (fetchCoroutine != null) { return; }

            try {
                fetchCoroutine = StartCoroutine(fetchAll(type));
            } catch (Exception e) {
                Debug.LogError(e);
                textField.text = @"ランキングの取得に失敗";
            } finally {
                fetchCoroutine = null;
            }
        }

        private IEnumerator fetchAll(RankingType type) {
            builder = new StringBuilder();

            while (true) { 
                currentRank.Reset();
                fetchRank(type, highScore.Value);
                yield return new WaitUntil(currentRank.TakeOrFailure);

                if (!currentRank.Failure) { break; }
                yield return new WaitForSeconds(3.0f);
            }

            while (true) { 
                topRankers.Reset();
                fetchRankers(type);
                yield return new WaitUntil(topRankers.TakeOrFailure);

                if (!topRankers.Failure) { break; }
                yield return new WaitForSeconds(3.0f);
            }
        }

        private void fetchRank(RankingType type, int score) {
            var className = getClassName(type);
            var query = new NCMBQuery<NCMBObject>(className);
            query.WhereGreaterThan(@"score", score);
            query.CountAsync((count, e) => {
                if (e != null) {
                    Debug.LogError(e);
                    currentRank.Exception = e;
                    return;
                }

                currentRank.Value = count + 1;

                builder.AppendLine($"あなたの順位: { currentRank.Value } 位");
                builder.AppendLine(@"============================");
                textField.text = builder.ToString();
            });
        }

        private void fetchRankers(RankingType type) {
            var className = getClassName(type);
            var query = new NCMBQuery<NCMBObject>(className);
            query.OrderByDescending(@"score");
            query.Limit = FetchCount;
            query.FindAsync((list, e) => {
                if (e != null) {
                    Debug.LogError(e);
                    topRankers.Exception = e;
                    return;
                }

                var rankers = new List<Ranker>();
                foreach (var obj in list) {
                    var name = Convert.ToString(obj["name"]);
                    var score = Convert.ToInt32(obj["score"]);
                    rankers.Add(new Ranker(name, score));
                }
                topRankers.Value = rankers;

                for (int i = 0; i < rankers.Count; i++) {
                    var str = rankers[i].ToString();

                    if (i < rankers.Count - 1) {
                        builder.AppendLine(str);
                    } else {
                        builder.Append(str);
                    }
                }

                textField.text = builder.ToString();
            });
        }

        public bool Save(RankingType type, Ranker ranker) {
            if (string.IsNullOrWhiteSpace(ranker.Name)) {
                throw new ArgumentException(@"空の名前でランキングに登録する事はできません。");
            }

            if (saveCoroutine != null) {
                StopCoroutine(saveCoroutine);
            }

            try {
                saveCoroutine = StartCoroutine(saveRank(type, ranker));
            } catch (Exception e) {
                Debug.LogError(e.Message);
                return false;
            } finally {
                saveCoroutine = null;
            }

            return true;
        }

        private IEnumerator saveRank(RankingType type, Ranker ranker) {
            var className = getClassName(type);
            var ncmbObj = new NCMBObject(className);
            ncmbObj[@"name"] = ranker.Name;
            ncmbObj[@"score"] = ranker.Score;

            var key = PlayerPrefsKey.ObjectId[type];
            if (PlayerPrefs.HasKey(key)) {
                ncmbObj.ObjectId = PlayerPrefs.GetString(key);
            }

            var watcher = new ASyncValue<bool, NCMBException>();

            while (true) {
                ncmbObj.SaveAsync(e => {
                    if (e != null) {
                        Debug.LogError(e);
                        watcher.Exception = e;
                        return;
                    }
                    watcher.Value = true;
                    PlayerPrefs.SetString(key, ncmbObj.ObjectId);
                });
                yield return new WaitUntil(watcher.TakeOrFailure);

                if (!watcher.Failure) { break; }
                yield return new WaitForSeconds(3.0f);
            }

            Fetch(type);
        }
    }
}