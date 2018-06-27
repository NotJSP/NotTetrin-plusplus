using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotTetrin.Title {
    public class TitleManager : MonoBehaviour {
        public static readonly float TransitionDuration = 0.4f;

        private void Update() {
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.D)) {
                PlayerPrefs.DeleteAll();
                Debug.Log(@"ローカルデータを削除しました。");
            }
        }
    }
}