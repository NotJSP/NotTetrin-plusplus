using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotTetrin.Ingame.MultiPlay.Marathon {
    [RequireComponent(typeof(Renderer))]
    public class CollidersGroupView : MonoBehaviour {
        private new Renderer renderer;
        private bool firstRead = true;
        private bool firstWrite = true;

        private void Awake() {
            renderer = GetComponent<Renderer>();
        }

        private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
            if (stream.isWriting) {
                serialize(stream, info);
            } else {
                deserialize(stream, info);
            }
        }

        private void serialize(PhotonStream stream, PhotonMessageInfo info) {
            if (firstWrite) {
                serializeFirst(stream, info);
            }
            var enabled = renderer.enabled;
            stream.SendNext(enabled);
        }

        private void serializeFirst(PhotonStream stream, PhotonMessageInfo info) {
            var parent = transform.parent.name;
            stream.SendNext(parent);
            var position = transform.position;
            stream.SendNext(position);
            var scale = transform.localScale;
            stream.SendNext(scale);

            firstWrite = false;
        }


        private void deserialize(PhotonStream stream, PhotonMessageInfo info) {
            if (firstRead) {
                deserializeFirst(stream, info);
            }
            var enabled = (bool)stream.ReceiveNext();
            renderer.enabled = enabled;
        }

        private void deserializeFirst(PhotonStream stream, PhotonMessageInfo info) {
            var parent = (string)stream.ReceiveNext();
            transform.parent = GameObject.Find(parent).transform;
            var position = (Vector3)stream.ReceiveNext();
            transform.position = position;
            var scale = (Vector3)stream.ReceiveNext();
            transform.localScale = scale;

            firstRead = false;
        }
    }
}