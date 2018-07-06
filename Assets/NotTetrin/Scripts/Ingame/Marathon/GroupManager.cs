using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotTetrin.Ingame.Marathon {
    public class GroupManager : MonoBehaviour {
        [SerializeField] private Director director;
        [SerializeField] private Instantiator instantiator;
        [SerializeField] private IngameSfxManager sfxManager;

        private CollidersGroup[] groups;
        public event EventHandler<DeleteMinoInfo> MinoDeleted;

        private void Awake() {
            var objects = director.CollidersField.Create();
            groups = objects.Select(o => o.GetComponent<CollidersGroup>()).ToArray();
            foreach (var group in groups) {
                group.Initialize(instantiator);
            }
        }

        public void DeleteMino() {
            var eraseGroups = groups.Where(g => g.IsEntered);
            var lineCount = eraseGroups.Count();
            var objectCount = eraseGroups.Sum(g => g.EnteredObjectCount);
            if (lineCount == 0) { return; }

            foreach (var group in eraseGroups) {
                group.DeleteMino();
            }

            var info = new DeleteMinoInfo(lineCount, objectCount);
            MinoDeleted?.Invoke(this, info);
            sfxManager.Play(IngameSfxType.MinoDelete);
        }
    }
}