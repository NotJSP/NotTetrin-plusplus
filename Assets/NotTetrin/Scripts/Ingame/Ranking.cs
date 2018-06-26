using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NCMB;
using NotTetrin.Constants;

namespace NotTetrin.Ingame {
    public class Ranking : MonoBehaviour {
        private static readonly int FetchCount = 10;

        [SerializeField]
        private Text textField;
        [SerializeField]
        private HighScore highScore;

        private int? currentRank;
        private List<Ranker> rankers = new List<Ranker>();

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
            textField.text = @"ランキング取得中...";
            try {
                StartCoroutine(fetchAll(type));
            } catch (Exception e) {
                Debug.LogError(e.Message);
                textField.text = @"ランキングの取得に失敗";
            }
        }

        private IEnumerator fetchAll(RankingType type) {
            builder = new StringBuilder();
            fetchRank(type, highScore.Value);
            yield return new WaitUntil(() => currentRank.HasValue);
            fetchRankers(type);
        }

        private void fetchRank(RankingType type, int score) {
            var className = getClassName(type);
            var query = new NCMBQuery<NCMBObject>(className);
            query.WhereGreaterThan(@"score", score);
            query.CountAsync((count, e) => {
                if (e != null) {
                    Debug.LogError(e.Message);
                } else {
                    currentRank = count + 1;
                }
                builder.AppendLine($"あなたの順位: { currentRank } 位");
                builder.AppendLine(@"========================");
                textField.text = builder.ToString();
            });
        }

        private void fetchRankers(RankingType type) {
            var className = getClassName(type);
            var query = new NCMBQuery<NCMBObject>(className);
            query.OrderByDescending(@"score");
            query.Limit = FetchCount;
            query.FindAsync((objList, e) => {
                if (e != null) {
                    Debug.LogError(e.Message);
                } else {
                    var list = new List<Ranker>();
                    foreach (var obj in objList) {
                        var name = Convert.ToString(obj["name"]);
                        var score = Convert.ToInt32(obj["score"]);
                        list.Add(new Ranker(name, score));
                    }
                    rankers = list;
                }

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
            try {
                var className = getClassName(type);
                var ncmbObj = new NCMBObject(className);
                ncmbObj[@"name"] = ranker.Name;
                ncmbObj[@"score"] = ranker.Score;

                var key = PlayerPrefsKey.ObjectId[type];
                if (PlayerPrefs.HasKey(key)) {
                    ncmbObj.ObjectId = PlayerPrefs.GetString(key);
                    ncmbObj.SaveAsync();
                } else {
                    ncmbObj.SaveAsync(e => PlayerPrefs.SetString(key, ncmbObj.ObjectId));
                }
            } catch (Exception e) {
                Debug.LogError(e.Message);
                return false;
            }

            return true;
        }
    }
}