using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NotTetrin.SceneManagement;

namespace NotTetrin.Title {
    public class NetworkBattleButton : MonoBehaviour {
        [SerializeField]
        private InputField nameField;

        public void OnPressed() {
            // TODO: 名前が空だった時の処理
            if (string.IsNullOrWhiteSpace(nameField.text)) {
                return;
            }
            PlayerPrefs.SetString(@"player_name", nameField.text);

            SceneTransit.Instance.LoadScene(@"Matching", TitleManager.TransitionDuration);
        }
    }
}