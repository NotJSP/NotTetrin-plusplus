using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NotTetrin.Ingame {
    [RequireComponent(typeof(AudioSource))]
    public class BGMManager : MonoBehaviour {
        [SerializeField]
        private AudioClip[] clips;

        private AudioSource audioSource;

        private void Awake() {
            audioSource = GetComponent<AudioSource>();
        }

        public void Stop() {
            audioSource.Stop();
        }

        public void RandomPlay() {
            // クリップがない場合、無音
            if (clips.Length == 0) { return; }

            var index = Random.Range(0, clips.Length - 1);
            audioSource.clip = clips[index];
            audioSource.Play();
        }
    }
}
