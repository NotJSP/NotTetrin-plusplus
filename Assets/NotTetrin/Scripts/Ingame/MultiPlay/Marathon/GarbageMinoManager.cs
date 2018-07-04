using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using NotTetrin.Utility;

namespace NotTetrin.Ingame.MultiPlay.Marathon {
    public class GarbageMinoManager : MonoBehaviour {
        [SerializeField] Instantiator instantiator;
        [SerializeField] MinoResolver resolver;
        [SerializeField] MinoSpawner spawner;
        [SerializeField] Rigidbody2D minoRigidbody;

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
                var obj = spawner.Spawn(index);
                var rigidbody = obj.AddComponent<Rigidbody2D>().CopyOf(minoRigidbody);
                var torque = Random.Range(-180f, 180f);
                rigidbody.AddTorque(torque);
                garbages.Add(obj);
                yield return new WaitForSeconds(0.5f);
            }
            yield return new WaitForSeconds(0.25f);
            IsFalling = false;
        }

        public void Add(int lines) {
            readyGarbageCount += lines;
        }
    }
}
