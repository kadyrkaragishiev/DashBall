using System;
using System.Collections.Generic;
using kadyrkaragishiev.LevelingSystem;
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

        [SerializeField]
        private GameObject bumpingEffect;

        [SerializeField]
        private BrickAudioController _brickAudioController;

        private bool _controllable = true;
        private Vector3 _moveDirection = Vector3.zero;
        private Vector3 _lastPosition;
        private float _myRadius;
        private bool _isDashing;
        private bool _won;
        private bool _destroyed;


        public Dictionary<int, Platform> _platforms = new();
        private int _currentPlatform = 0;

        private void Start()
        {
            _myRadius = transform.localScale.x / 2;
            _lastPosition = transform.position;
            LevelManager.Instance.OnGameInit += InstanceOnOnGameInit;
        }

        private void InstanceOnOnGameInit(LevelSettings settings)
        {
            _won = false;
            _destroyed = false;
            _controllable = true;
            _isDashing = false;
            _currentPlatform = 0;
            _moveDirection = Vector3.zero;
            _lastPosition = transform.position;
        }


        private void Update()
        {
            if (_won) return;
            if (_destroyed) return;
            _moveDirection.y -= gravityPower * Time.deltaTime;
            transform.position += _moveDirection * Time.deltaTime;
            if (_controllable)
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
                if (hit.transform.CompareTag("Finish Line"))
                {
                    ReachFinishLine();
                    return;
                }

                if (hit.transform.parent.TryGetComponent(out Platform platform))
                    Instantiate(bumpingEffect, hit.point, Quaternion.identity);
                _moveDirection.y = bouncePower;
                transform.position = new Vector3(transform.position.x, hit.point.y + _myRadius, transform.position.z);
            }
        }

        private void Dash()
        {
            _moveDirection.y -= dashSpeed;
            if (!Physics.Linecast(_lastPosition, transform.position - new Vector3(0, _myRadius, 0),
                    out var hit)) return;
            if (_currentPlatform < _platforms.Count)
            {
                _brickAudioController.PlayRandomClip();
                _platforms[_currentPlatform].DestroyPlatform();
                if (hit.transform.parent.TryGetComponent(out Platform platform))
                {
                    if (platform.IsDamageTile(hit.transform.gameObject))
                    {
                        transform.position = hit.point + new Vector3(0, _myRadius, 0);
                        DestroyMe();
                        return;
                    }
                }

                _currentPlatform += 1;
            }
            else
            {
                if (hit.transform.gameObject.CompareTag("Finish Line"))
                {
                    ReachFinishLine();
                    _moveDirection = Vector3.zero;
                    transform.position = hit.point;
                }
            }
        }

        private void ReachFinishLine()
        {
            Debug.Log("FINISH LINE");
            _won = true;
            _isDashing = false;
            _controllable = false;
            OnBoardingBehaviour.Instance.CallOnBoarding("NextLevel");
            gameObject.SetActive(false);
            ProgressBehaviour.Instance.Progress++;
        }

        private void DestroyMe()
        {
            OnBoardingBehaviour.Instance.CallOnBoarding("RestartPage");
            _destroyed = true;
            _platforms.Clear();
            var t = Instantiate(Explode, transform.position, transform.rotation);
            Destroy(t, 2f);
            gameObject.SetActive(false);
        }
    }
}