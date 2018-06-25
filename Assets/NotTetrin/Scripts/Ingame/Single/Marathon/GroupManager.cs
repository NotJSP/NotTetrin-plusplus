using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotTetrin.Ingame.Single.Marathon {
    public class GroupManager : MonoBehaviour {
        public GameObject[] group;

        void Start() {
            group = GameObject.FindGameObjectsWithTag("GroupField");
        }

        public void DeleteMino() {
            for (int i = 0; i < group.Length; i++) {
                group[i].GetComponent<CreateTileAndGrouping>().DeleteMino();
            }
        }
    }
}