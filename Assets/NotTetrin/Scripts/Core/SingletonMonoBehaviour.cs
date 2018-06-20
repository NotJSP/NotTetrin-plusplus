using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotTetrin {
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour {
        private static T instance;

        public static T Instance {
            get {
                if (instance == null) {
                    instance = FindObjectOfType<T>();

                    if (instance == null) {
                        Debug.LogError(typeof(T) + " is nothing");
                    }
                }

                return instance;
            }
        }
    }
}