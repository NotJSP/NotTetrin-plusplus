using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NotTetrin.UI;
using NotTetrin.Constants;
using NotTetrin.SceneManagement;

namespace NotTetrin.Title {
    public class StackModeButton : MonoBehaviour {
        [SerializeField]
        private InputField nameField;
        [SerializeField]
        private SelectableToggler toggler;

        public void OnPressed() {
            // TODO: 名前が空だった時の処理
            if (string.IsNullOrWhiteSpace(nameField.text)) {
                return;
            }
            PlayerPrefs.SetString(PlayerPrefsKey.PlayerName, nameField.text);

            toggler.ToggleAll();
            SceneTransit.Instance.LoadScene(SceneName.StackMode, TitleManager.TransitionDuration);
        }
    }
}