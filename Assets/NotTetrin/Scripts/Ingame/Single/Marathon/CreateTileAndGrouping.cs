using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace NotTetrin.Ingame.Single.Marathon {
    [DefaultExecutionOrder(1)]
    [RequireComponent(typeof(CreateTile))]
    public class CreateTileAndGrouping : MonoBehaviour {
        [SerializeField]
        private Text text;

        private List<GameObject> minos = new List<GameObject>();

        private new Renderer renderer;

        public ColliderGroup group;

        private bool minomoving;

        private void Awake() {
            renderer = GetComponent<Renderer>();

            // 作ったタイルの子供達のColliderHelperを取得して配列にする
            var colliders = GetComponent<CreateTile>().children
                .Select(o => o.GetComponent<ColliderHelper>())
                .ToArray();
            group = new ColliderGroup(colliders);
        }

        public void Update() {
            renderer.enabled = group.EnteredAll;
        }


        public void DeleteMino() {
            if (group.EnteredAll) {
                Debug.Log("ミノ削除");

                while (minos.Count > 0) {
                    Debug.Log(minos[0] + "削除");
                    minos[0].transform.Translate(10000, 10000, 10000);
                    Destroy(minos[0], 1.0f); //即消しだと判定が残るから、時間差攻撃。
                    minos.Remove(minos[0]);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision) {
            minos.Add(collision.gameObject);
        }

        private void OnTriggerExit2D(Collider2D collision) {
            minos.Remove(collision.gameObject);
        }
    }
}