using System;
using System.Collections.Generic;
using UnityEngine;

namespace NotTetrin.Ingame {
    public class MinoManager : MonoBehaviour {
        [SerializeField] private Instantiator instantiator;
        [SerializeField] private Director director;
        [SerializeField] private MinoSpawner spawner;
        [SerializeField] private IngameAudioManager audioManager;
        [SerializeField] private Rigidbody2D minoRigidbody;

        private int currentIndex;
        private bool useHold = false;

        public event EventHandler HitMino;

        private List<GameObject> minos = new List<GameObject>();
        public GameObject CurrentMino => minos.Count != 0 ? minos[minos.Count - 1] : null;

        private void Update() {
            if (!useHold && Input.GetButtonDown(@"Hold")) {
                hold();
            }
        }

        public void Reset() {
            spawner.Clear();

            director.HoldMino.Clear();
            useHold = false;

            minos.ForEach(instantiator.Destroy);
            minos.Clear();
        }

        public void Next() {
            var obj = spawner.Next();
            change(obj);

            useHold = false;
        }

        private void hold() {
            int? value = director.HoldMino.Value;
            director.HoldMino.Hold(spawner.LastIndex);

            Destroy();

            if (value == null) {
                var obj = spawner.Next();
                change(obj);
            } else {
                var obj = spawner.Spawn(value.Value);
                change(obj);
            }

            useHold = true;
        }

        public void Release() {
            var controller = CurrentMino.GetComponent<MinoController>();
            Destroy(controller);
        }

        public void Destroy() {
            instantiator.Destroy(CurrentMino);
            minos.RemoveAt(minos.Count - 1);
        }

        private void change(GameObject mino) {
            mino.AddComponent<Rigidbody2D>().Copy(minoRigidbody);
            var controller = mino.AddComponent<MinoController>().Initialize(audioManager);
            controller.Hit += onHitMino;

            minos.Add(mino);
        }

        private void onHitMino(object sender, EventArgs args) {
            HitMino.Invoke(sender, args);
        }
    }
}
