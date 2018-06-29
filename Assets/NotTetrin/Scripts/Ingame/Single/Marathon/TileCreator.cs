using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotTetrin.Ingame.Single.Marathon {
    public class TileCreator : MonoBehaviour {
        [Header(@"Objects")]
        public GameObject Prefab;
        public GameObject Field;

        [Header(@"Properties")]
        public bool CreateOnAwake;
        [Range(1, 100)]
        public int DivideNumX = 10;
        [Range(1, 100)]
        public int DivideNumY = 10;

        public GameObject[] Children { get; private set; }

        private void Awake() {
            if (CreateOnAwake) {
                Create();
            }
        }

        public void Create() {
            var rect = (Field == null) ? gameObject.rect() : Field.rect();
            var size = Prefab.size();
            var helper = new TileHelper().Calculate(rect, size, DivideNumX, DivideNumY);

            Children = new GameObject[DivideNumX * DivideNumY];

            for (int j = 0; j < DivideNumY; j++) {
                var y = (helper.Unit.y / 2) + rect.yMin + helper.Unit.y * j;
                for (int i = 0; i < DivideNumX; i++) {
                    var x = (helper.Unit.x / 2) + rect.xMin + helper.Unit.x * i;
                    var position = new Vector3(x, y);
                    var obj = Instantiate(Prefab, position, Quaternion.identity);
                    obj.transform.localScale = new Vector3(obj.transform.localScale.x * helper.Rate.x, obj.transform.localScale.y * helper.Rate.y);
                    obj.transform.SetParent(transform);
                    Children[i + j * DivideNumX] = obj;
                }
            }
        }
    }
}