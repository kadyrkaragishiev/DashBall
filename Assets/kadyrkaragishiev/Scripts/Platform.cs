using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kadyrkaragishiev.Scripts
{
    public class Platform : MonoBehaviour
    {
        public delegate void OnDisableCallback(Platform platform);

        [SerializeField]
        private List<MeshRenderer> tileMeshRenderer = new List<MeshRenderer>();

        [SerializeField]
        private List<Rigidbody> tileRigidbody = new List<Rigidbody>();

        [SerializeField]
        private List<Collider> _colliders;

        private Dictionary<GameObject, bool> _tileDictionary = new Dictionary<GameObject, bool>();

        public void Initialize(List<int> damageTileIndecies, Material normalMat, Material damageMat)
        {
            for (var i = 0; i < tileMeshRenderer.Count; i++)
            {
                if (damageTileIndecies.Contains(i))
                {
                    tileMeshRenderer[i].material = damageMat;
                    tileMeshRenderer[i].tag = "Damage Tile";
                    _tileDictionary.Add(tileMeshRenderer[i].gameObject, true);
                }
                else
                {
                    tileMeshRenderer[i].material = normalMat;
                    tileMeshRenderer[i].tag = "Normal Tile";
                    _tileDictionary.Add(tileMeshRenderer[i].gameObject, false);
                }
            }
        }

        public bool IsDamageTile(GameObject tile) => _tileDictionary[tile];

        public void DestroyPlatform()
        {
            transform.parent = null;
            for (var i = 0; i < tileRigidbody.Count; i++)
            {
                var body = tileRigidbody[i];
                body.isKinematic = false;
                body.AddForce((_colliders[i].gameObject.transform.forward) * 10 + Vector3.up * 5, ForceMode.Impulse);
                _colliders[i].enabled = false;
            }

            StartCoroutine(CallDisable());
        }

        private IEnumerator CallDisable()
        {
            yield return new WaitForSeconds(0.5f);
            Destroy(gameObject);
        }
    }
}