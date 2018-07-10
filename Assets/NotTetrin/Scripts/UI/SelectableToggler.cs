using UnityEngine;
using UnityEngine.UI;

namespace NotTetrin.UI { 
    public class SelectableToggler : MonoBehaviour {
        [SerializeField] CanvasGroup[] groups;
        [SerializeField] Selectable[] objects;

        public void ToggleAll() {
            foreach (var group in groups) {
                group.interactable = !group.interactable;
            }
            foreach (var obj in objects) {
                obj.interactable = !obj.interactable;
            }
        }
    }
}
