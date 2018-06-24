using UnityEngine;
using UnityEngine.UI;

namespace NotTetrin.UI { 
    public class SelectableToggler : MonoBehaviour {
        [SerializeField]
        private Selectable[] objects;

        public void ToggleAll() {
            foreach (var obj in objects) {
                obj.interactable = !obj.interactable;
            }
        }
    }
}
