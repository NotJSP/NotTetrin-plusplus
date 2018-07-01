using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NotTetrin.SceneManagement {
    public class SceneBase : MonoBehaviour {
        protected virtual void Awake() {
            SceneController.Instance.SceneReady += OnSceneReady;
        }

        protected virtual void OnDestroy() {
            SceneController.Instance.SceneReady -= OnSceneReady;
        }

        protected virtual void OnSceneReady(object sender, EventArgs args) { }
    }
}
