using UnityEngine;

namespace kadyrkaragishiev.Scripts
{
    public class AutoRotator : MonoBehaviour
    {
        [SerializeField]
        private Vector3 _rotateDir;

        private void Update()
        {
            transform.Rotate(_rotateDir * Time.deltaTime);
        }
    }
}
