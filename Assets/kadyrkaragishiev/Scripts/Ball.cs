using System.Collections.Generic;
using UnityEngine;

namespace kadyrkaragishiev.Scripts
{
    public class Ball : MonoBehaviour
    {
        [SerializeField]
        private float gravityPower = 5f;

        [SerializeField]
        private float bouncePower = 1f;

        [SerializeField]
        private float dashSpeed = 10f;

        [SerializeField]
        private GameObject Explode;
        
        private bool controllable = true;
        private Vector3 _moveDirection = Vector3.zero;
        private Vector3 _lastPosition;
        private float _myRadius;
        private bool _isDashing;
        private bool won;
        private bool destroyed;


        public Dictionary<int, Platform> _platforms = new Dictionary<int, Platform>();
        private int currentPlaftorm = -1;

        private void Start()
        {
            _myRadius = transform.localScale.x / 2;
            _lastPosition = transform.position;
        }

        private void Update()
        {
            if(destroyed) return;
            _moveDirection.y -= gravityPower * Time.deltaTime;
            transform.position += _moveDirection * Time.deltaTime;
            if (controllable)
            {
                if (Input.GetMouseButtonDown(0))
                    _isDashing = true;
                if (Input.GetMouseButtonUp(0))
                    _isDashing = false;
            }

            if (_isDashing)
                Dash();
            else
                Idle();
            _lastPosition = transform.position;
        }

        private void Idle()
        {
            if (Physics.Linecast(_lastPosition, transform.position - new Vector3(0, _myRadius, 0), out var hit))
            {
                if (hit.transform.CompareTag("Finish Line")) ReachFinishLine();
                _moveDirection.y = bouncePower;
                transform.position = new Vector3(transform.position.x, hit.point.y + _myRadius, transform.position.z);
            }
        }

        private void Dash()
        {
            _moveDirection.y -= dashSpeed;
            if (Physics.Linecast(_lastPosition, transform.position - new Vector3(0, _myRadius, 0), out var hit))
            {
                if (currentPlaftorm < _platforms.Count)
                {
                    if (hit.transform.CompareTag("Damage Tile"))
                    {
                        transform.position = hit.point + new Vector3(0, _myRadius, 0);
                        DestroyMe();
                        return;
                    }
                    currentPlaftorm += 1;
                    _platforms[currentPlaftorm].DestroyPlatform();
                }
                else
                {
                    Debug.Log("Call Win");
                    ReachFinishLine();
                    _moveDirection = Vector3.zero;
                    transform.position = hit.point;
                }
            }
        }

        private void ReachFinishLine()
        {
            _isDashing = false;
            controllable = false;
        }

        private void DestroyMe()
        {
            //TODO: Add destroy animation
            destroyed = true;
            Instantiate(Explode, transform.position, transform.rotation);
            gameObject.SetActive(false);
        }
    }
}