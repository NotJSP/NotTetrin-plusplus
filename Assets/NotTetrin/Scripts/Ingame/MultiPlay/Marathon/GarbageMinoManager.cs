using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using NotTetrin.Utility;

namespace NotTetrin.Ingame.MultiPlay.Marathon {
    public class GarbageMinoManager : MonoBehaviour {
        [Header(@"References")]
        [SerializeField] Instantiator instantiator;
        [SerializeField] MinoResolver resolver;
        [SerializeField] MinoSpawner spawner;
        [SerializeField] Rigidbody2D minoRigidbody;

        [Header(@"Properties")]
        [SerializeField] float offsetRange;
        [SerializeField] float torqueRange;
        [SerializeField] Vector2 force;

        private List<GameObject> garbages = new List<GameObject>();
        private int readyGarbageCount;
        public bool IsFalling { get; private set; }

        public void Clear() {
            garbages.ForEach(instantiator.Destroy);
            readyGarbageCount = 0;
        }

        public bool Fall() {
            if (readyGarbageCount == 0) { return false; }

            IsFalling = true;
            StartCoroutine(fallCoroutine(readyGarbageCount));
            readyGarbageCount = 0;

            return true;
        }

        private IEnumerator fallCoroutine(int count) {
            for (int i = 0; i < count; i++) {
                var index = Random.Range(0, resolver.Length);
                var offset = Random.Range(-offsetRange, offsetRange);
                var obj = spawner.Spawn(index, offset);
                var rigidbody = obj.AddComponent<Rigidbody2D>().CopyOf(minoRigidbody);
                var torque = Random.Range(-torqueRange, torqueRange);
                rigidbody.AddTorque(torque);
                rigidbody.AddForce(force);
                garbages.Add(obj);
                yield return new WaitForSeconds(0.7f);
            }
            yield return new WaitForSeconds(0.35f);
            IsFalling = false;
        }

        public void Add(DeleteMinoInfo info) {
            Debug.Log($"lines: {info.LineCount}, objects: {info.ObjectCount}");
            var amount = info.LineCount + info.ObjectCount / 7;
            Debug.Log(amount);
            readyGarbageCount += amount;
        }
    }
}
