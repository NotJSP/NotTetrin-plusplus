using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace NotTetrin.Ingame.Single.Marathon {
    /**
     * タイルを作ってグルーピングするクラス
     * TileCreatorコンポーネントのCreateOnAwakeをオフにすること!!!
     */
    [DefaultExecutionOrder(1)]
    [RequireComponent(typeof(Renderer), typeof(TileCreator))]
    public class CreateTileAndGrouping : MonoBehaviour {
        [SerializeField] private DensityIndicator indicator;

        private new Renderer renderer;
        private List<GameObject> minos = new List<GameObject>();
        private ColliderGroup group;

        public bool IsEntered => group.EnteredAll;

        private ParticleSystem MinoDeleteEffect;

        private void Awake() {
            renderer = GetComponent<Renderer>();

            // タイル生成
            var creator = GetComponent<TileCreator>();
            creator.Create();

            // 作ったタイルの子供達のColliderHelperを取得
            var colliders = creator.Children.Select(o => o.GetComponent<ColliderHelper>());
            group = new ColliderGroup(colliders);

            MinoDeleteEffect = this.GetComponentInChildren<ParticleSystem>();
            MinoDeleteEffect.Stop();
        }

        public void Update() {
            // var density = (float)group.EnterCount / group.Children.Length;
            // indicator.UpdateDensity(density);
            renderer.enabled = IsEntered;
        }

        public void DeleteMino() {
            Debug.Log("ミノ削除");
            MinoDeleteEffect.Play();
            foreach (var mino in minos) {
                Debug.Log(mino + "削除");
                mino.transform.Translate(10000, 10000, 10000);  // OnTriggerExit2Dを呼び出すため範囲外へ移動
                Destroy(mino, 1.0f);    // 即消しだと判定が残るから時間差攻撃
            }
            minos.Clear();
        }

        private void OnTriggerEnter2D(Collider2D collision) {
            minos.Add(collision.gameObject);
        }

        private void OnTriggerExit2D(Collider2D collision) {
            minos.Remove(collision.gameObject);
        }
    }
}