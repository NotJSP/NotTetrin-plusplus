using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NotTetrin.Constants;

namespace NotTetrin.Title {
    public class TitleManager : MonoBehaviour {
        [SerializeField] private InputField nameField;

        public static readonly float TransitionDuration = 1.0f;

        private void Awake() {
            // TODO: 本番では保存しない
            if (PlayerPrefs.HasKey(PlayerPrefsKey.PlayerName)) {
                nameField.text = PlayerPrefs.GetString(PlayerPrefsKey.PlayerName);
            }
        }

        private void Update() {
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.D)) {
                PlayerPrefs.DeleteAll();
                Debug.Log(@"ローカルデータを削除しました。");
            }
        }
    }
}