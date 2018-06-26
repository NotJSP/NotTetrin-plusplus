using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotTetrin.Ingame {
    public class HoldMino : MonoBehaviour {
        [SerializeField] private Instantiator instantiator;
        [SerializeField] private IngameSfxManager sfxManager;
        [SerializeField] private MinoResolver resolver;
        [SerializeField] private Transform frame;

        private Animator animator;

        private GameObject mino;

        public int? Value { get; private set; } = null;

        private void Awake() {
            animator = GetComponent<Animator>();
        }

        public void Clear() {
            if (mino != null) {
                instantiator.Destroy(mino);
            }
            Value = null;
        }

        public void Hold(int index) {
            Clear();

            mino = instantiator.Instantiate(resolver.Get(index), frame.position, Quaternion.identity);
            instantiator.Destroy(mino.transform.GetChild(0).gameObject);
//            obj.transform.parent = transform;

            Value = index;

            animator.Play(@"HoldAnimation", 0, 0.0f);
            sfxManager.Play(IngameSfxType.MinoHold);
        }
    }
}