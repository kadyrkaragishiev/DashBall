using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kadyrkaragishiev.Scripts
{
    public class Platform : MonoBehaviour
    {
        [SerializeField]
        private List<MeshRenderer> tileMeshRenderer = new List<MeshRenderer>();

        [SerializeField]
        private List<Rigidbody> tileRigidbody = new List<Rigidbody>();

        public delegate void OnDisableCallback(Platform platform);

        public OnDisableCallback OnDisableEvent;

        public void Initialize(List<int> damageTileIndecies, Material normalMat, Material damageMat)
        {
            for (var i = 0; i < tileMeshRenderer.Count; i++)
            {
                tileMeshRenderer[i].material = damageTileIndecies.Contains(i) ? damageMat : normalMat;
                tileMeshRenderer[i].tag = damageTileIndecies.Contains(i) ? "Damage Tile" : "Normal Tile";
            }
        }

        public void DestroyPlatform()
        {
            foreach (var body in tileRigidbody)
            {
                body.isKinematic = false;
                body.AddExplosionForce(1000, transform.position - new Vector3(0, 1, 0), 2);
            }

            StartCoroutine(CallDisable());
            // OnDisableEvent?.Invoke(this);
            // Destroy(gameObject, 1f);
        }

        private IEnumerator CallDisable()
        {
            yield return new WaitForSeconds(1f);
            OnDisableEvent?.Invoke(this);
        }
    }
}