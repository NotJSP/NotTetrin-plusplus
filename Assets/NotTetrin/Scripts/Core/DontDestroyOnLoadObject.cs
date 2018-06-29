using UnityEngine;

namespace NotTetrin {
    public class DontDestroyOnLoadObject : MonoBehaviour {
        private static GameObject obj;

        private void Awake() {
            if (obj != null && obj != gameObject) {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
            obj = gameObject;
        }
    }
}
