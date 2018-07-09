using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace NotTetrin.UI {
    [RequireComponent(typeof(Animator))]
    public class MessageWindow : MonoBehaviour {
        [SerializeField] Text messageText;
        private Animator animator;

        private void Awake() {
            animator = GetComponent<Animator>();
        }

        public void Show(string message) {
//            messageText.text = message;

            enableObject();
            animator.Play(@"WindowOpen");
        }

        public void Hide() {
            animator.Play(@"WindowClose");
            var state = animator.GetCurrentAnimatorStateInfo(0);
            Invoke(@"disableObject", state.length);
        }

        private void enableObject() {
            gameObject.SetActive(true);
        }

        private void disableObject() {
            gameObject.SetActive(false);
        }
    }
}
