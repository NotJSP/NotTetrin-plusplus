using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NotTetrin.Utility;

namespace NotTetrin.Ingame {
    public class NextMino : MonoBehaviour {
        [SerializeField] private Instantiator instantiator;
        [SerializeField] private Transform[] frames;
        [SerializeField] private MinoResolver resolver;

        private List<GameObject> objects = new List<GameObject>();

        private int[] indices;

        [HideInInspector]
        public List<int> NextIndices { get; private set; } = new List<int>();

        private void Awake() {
            indices = new int[resolver.Length];
            for (int i = 0; i < indices.Length; i++) {
                indices[i] = i;
            }
        }

        public void Clear() {
            NextIndices.Clear();
        }

        public int Next() {
            if (NextIndices.Count < resolver.Length) {
                enqueue();
            }

            var index = NextIndices[0];
            NextIndices.RemoveAt(0);

            updateView();

            return index;
        }

        private void updateView() {
            foreach (var obj in objects) {
                instantiator.Destroy(obj);
            }
            objects.Clear();

            for (int i = 0; i < frames.Length; i++) {
                var index = NextIndices[i];
                var obj = instantiator.Instantiate(resolver.Get(index), frames[i].position, Quaternion.identity);
                if (i > 0) {
                    obj.transform.localScale *= 0.6f;
                }
                Destroy(obj.transform.GetChild(0).gameObject);
                //obj.transform.parent = transform;

                objects.Add(obj);
            }
        }

        private void enqueue() {
            NextIndices.AddRange(indices.Shuffle());
        }
    }
}