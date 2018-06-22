using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotTetrin.Ingame.Single.Marethon
{
    public class GroupManager : MonoBehaviour {

        public GameObject[] group;

        // Use this for initialization
        void Start() {
            group = GameObject.FindGameObjectsWithTag("GroupField");
        }

        // Update is called once per frame
        void Update() {

        }

        public void DeleteMino()
        {
            for (int i = 0; i < group.Length; i++)
            {
                group[i].GetComponent<CreateTileAndGrouping>().DeleteMino();
            }
        }
    }
}