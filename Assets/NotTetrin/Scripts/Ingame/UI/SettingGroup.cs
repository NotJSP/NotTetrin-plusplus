using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NotTetrin.Ingame.UI {
    public class SettingGroup : MonoBehaviour {
        public Text Header;
        public RectTransform Container;

        public void SetHeader(string text) {
            Header.text = text;
        }
    }
}