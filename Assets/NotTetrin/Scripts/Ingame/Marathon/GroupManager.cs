using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NotTetrin.Utility.Physics2D;

namespace NotTetrin.Ingame.Marathon {
    public class GroupManager : MonoBehaviour {
        [SerializeField] private Director director;
        [SerializeField] private Instantiator instantiator;
        [SerializeField] private IngameSfxManager sfxManager;

        public CreateTileAndGrouping[] groups;
        public event EventHandler<int> MinoDeleted;

        private void Awake() {
            var field = director.CollidersField;
            field.GetComponent<TileCreator>().Create();

            groups = field.GetComponentsInChildren<CreateTileAndGrouping>();
            foreach (var group in groups) {
                group.Initialize(instantiator);
            }
        }

        public void DeleteMino() {
            var eraseGroups = groups.Where(g => g.IsEntered);
            var lines = eraseGroups.Count();
            if (lines == 0) { return; }

            foreach (var group in eraseGroups) {
                group.DeleteMino();
            }

            MinoDeleted?.Invoke(this, lines);
            sfxManager.Play(IngameSfxType.MinoDelete);
        }
    }
}