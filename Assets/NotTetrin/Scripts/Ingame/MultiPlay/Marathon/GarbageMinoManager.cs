﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using NotTetrin.Utility;
using NotTetrin.Ingame.Marathon;

using Random = UnityEngine.Random;

namespace NotTetrin.Ingame.MultiPlay.Marathon {
    public class GarbageMinoManager : MonoBehaviour {
        [SerializeField] Instantiator instantiator;
        [SerializeField] NetworkMarathonDirector director;
        [SerializeField] MinoResolver resolver;
        [SerializeField] MinoSpawner spawner;
        [SerializeField] Rigidbody2D minoRigidbody;

        private static float OffsetRange = 0.9f;
        private static float TorqueRange = 150.0f;
        private static Vector2 ForceGarbage = new Vector2(0, -100.0f);

        private List<GameObject> garbages = new List<GameObject>();
        private int readyGarbageCount;
        private float elapsedTime;

        public bool IsFalling { get; private set; }

        private int ReadyGarbageCount {
            get {
                return readyGarbageCount;
            }
            set {
                readyGarbageCount = value;
                updateIndicator();
            }
        }

        public void Clear() {
            garbages.ForEach(instantiator.Destroy);
            ReadyGarbageCount = 0;
            elapsedTime = 0.0f;
        }

        private void Update() {
            elapsedTime += Time.deltaTime;
        }

        public bool Fall() {
            if (ReadyGarbageCount == 0) { return false; }

            IsFalling = true;
            StartCoroutine(fallCoroutine(ReadyGarbageCount));
            ReadyGarbageCount = 0;
            updateIndicator();

            return true;
        }

        private IEnumerator fallCoroutine(int count) {
            for (int i = 0; i < count; i++) {
                var index = Random.Range(0, resolver.Length);
                var offset = Random.Range(-OffsetRange, OffsetRange);
                var obj = spawner.Spawn(index, offset);
                var rigidbody = obj.AddComponent<Rigidbody2D>().CopyOf(minoRigidbody);
                var torque = Random.Range(-TorqueRange, TorqueRange);
                rigidbody.AddTorque(torque);
                rigidbody.AddForce(ForceGarbage);
                garbages.Add(obj);
                yield return new WaitForSeconds(0.4f);
            }
            yield return new WaitForSeconds(0.8f);
            IsFalling = false;
        }

        public void Add(DeleteMinoInfo info) {
            Debug.Log($"lines: {info.LineCount}, objects: {info.ObjectCount}");
            var lineAmount = (float)info.LineCount;
            var objectAmount = (float)info.ObjectCount / 7;
            var cf = 1.0f + 0.25f * (int)(elapsedTime / 60);
            var amount = (lineAmount + objectAmount) * cf;
            Debug.Log(amount);
            ReadyGarbageCount += (int)amount;
        }

        private void updateIndicator() {
            director.GarbageIndicator.Value = readyGarbageCount;
        }
    }
}
