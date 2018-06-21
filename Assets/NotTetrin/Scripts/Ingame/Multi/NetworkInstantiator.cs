using UnityEngine;

namespace NotTetrin.Ingame.Multi {
    public class NetworkInstantiator : Instantiator {
        public override GameObject Instantiate(GameObject original, Vector3 position, Quaternion rotation, byte group) => PhotonNetwork.Instantiate(original.name, position, rotation, group);

        public override void Destroy(GameObject obj) {
            var view = obj.GetComponent<PhotonView>();
            if (view) {
                PhotonNetwork.Destroy(view);
            } else {
                Object.Destroy(obj);
            }
        }
    }
}
