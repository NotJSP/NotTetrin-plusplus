using UnityEngine;

namespace NotTetrin.Ingame.Single {
    public class LocalInstantiator : Instantiator {
        public override GameObject Instantiate(GameObject original, Vector3 position, Quaternion rotation, byte group) => Object.Instantiate(original, position, rotation);
        public override void Destroy(GameObject obj) => Object.Destroy(obj);
    }
}
