using System.Collections.Generic;
using UnityEngine;

namespace kadyrkaragishiev.Scripts
{
    public class Explode : MonoBehaviour
    {
        [SerializeField]
        private List<Rigidbody> rbs;

        [SerializeField]
        private float explosionPower = 20f;

        [SerializeField]
        private Vector3 explosionOffset;
        private void Start()
        {
            foreach (var t in rbs)
            {
                t.AddExplosionForce(explosionPower, transform.position + explosionOffset, 2f);
            }
        }

        private void Update()
        {
        
        }
    }
}
