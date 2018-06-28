using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotTetrin.Ingame.Single.Marathon {
    public class GroupManager : MonoBehaviour {
        [SerializeField] private Score score;
        [SerializeField] private IngameSfxManager sfxManager;

        public CreateTileAndGrouping[] groups;

        private void Start() {
            groups = GameObject.FindGameObjectsWithTag("GroupField").Select(o => o.GetComponent<CreateTileAndGrouping>()).ToArray();
        }

        public void DeleteMino() {
            var eraseGroups = groups.Where(g => g.IsEntered);
            var lines = eraseGroups.Count();
            if (lines == 0) { return; }

            var baseScore = 500.0;
            var amount = baseScore * Math.Pow(lines, 2) - baseScore * 0.15 * Math.Pow(lines, 2);
            score.Increase((int)amount);

            Debug.Log(eraseGroups.Count());
            foreach (var group in eraseGroups) {
                group.DeleteMino();
                sfxManager.Play(IngameSfxType.MinoDelete);
            }
        }
    }
}