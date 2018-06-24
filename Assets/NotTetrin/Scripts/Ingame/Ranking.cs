using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NCMB;

namespace NotTetrin.Ingame {
    public class Ranking : MonoBehaviour {
        private static readonly int FetchCount = 10;

        [SerializeField]
        private Text textField;
        [SerializeField]
        private HighScore highScore;

        private int currentRank = 0;
        private List<Ranker> rankers = new List<Ranker>();

        private StringBuilder builder;

        public void Fetch() {
            try {
                builder = new StringBuilder();
                fetchRank(highScore.Value);
                fetchRankers();
            } catch (Exception e) {
                Debug.LogError(e.Message);
                textField.text = @"ランキングの取得に失敗";
            }
        }

        private void fetchRank(int score) {
            var query = new NCMBQuery<NCMBObject>(@"Ranking");
            query.WhereGreaterThan(@"score", score);
            query.CountAsync((count, e) => {
                if (e != null) {
                    Debug.LogError(e.Message);
                } else {
                    this.currentRank = count + 1;
                }
                builder.AppendLine($"あなたの順位: { currentRank } 位");
                builder.AppendLine(@"========================");
                textField.text = builder.ToString();
            });
        }

        private void fetchRankers() {
            var query = new NCMBQuery<NCMBObject>(@"Ranking");
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
                    this.rankers = list;
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

        public bool Save(Ranker ranker) {
            try {
                var ncmbObj = new NCMBObject(@"Ranking");
                ncmbObj[@"name"] = ranker.Name;
                ncmbObj[@"score"] = ranker.Score;

                if (PlayerPrefs.HasKey(@"object_id")) {
                    ncmbObj.ObjectId = PlayerPrefs.GetString(@"object_id");
                    ncmbObj.SaveAsync();
                } else {
                    ncmbObj.SaveAsync(e => PlayerPrefs.SetString(@"object_id", ncmbObj.ObjectId));
                }
            } catch (Exception e) {
                Debug.LogError(e.Message);
                return false;
            }

            return true;
        }
    }
}