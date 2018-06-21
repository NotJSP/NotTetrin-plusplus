using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NotTetrin.SceneManagement;

namespace NotTetrin.Title {
    public class StackModeButton : MonoBehaviour {
        [SerializeField]
        private InputField nameField;

        public void OnPressed() {
            if (string.IsNullOrWhiteSpace(nameField.text)) { return; }
            PlayerPrefs.SetString(@"player_name", nameField.text);

            SceneTransit.Instance.LoadScene(@"StackMode", TitleManager.TransitionDuration);
        }
    }
}