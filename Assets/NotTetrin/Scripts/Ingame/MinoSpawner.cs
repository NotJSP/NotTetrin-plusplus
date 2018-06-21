using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotTetrin.Ingame {
    public class MinoSpawner : MonoBehaviour {
        [SerializeField]
        private Instantiator instantiator;
        [SerializeField]
        private Director director;
        [SerializeField]
        protected MinoResolver resolver;

        [HideInInspector]
        public int LastIndex { get; private set; }

        public event EventHandler OnSpawn;

        private Vector3 spawnPosition => director.Ceiling.transform.position;

        public void Clear() {
            director.NextMino.Clear();
        }

        public GameObject Next() {
            var index = director.NextMino.Next();
            return Spawn(index);
        }

        public GameObject Spawn(int index) {
            LastIndex = index;

            var obj = instantiator.Instantiate(resolver.Get(index), spawnPosition, Quaternion.identity);
            obj.transform.parent = transform;

            OnSpawn?.Invoke(this, EventArgs.Empty);
            return obj;
        }
    }
}