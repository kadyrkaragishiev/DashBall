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
        private GameObject Explode;

        [SerializeField]
        private GameObject bumpingEffect;

        [SerializeField]
        private BrickAudioController _brickAudioController;

        [SerializeField]
        private AudioSource bumpingSource;

        [SerializeField]
        private Rigidbody rb;

        private bool _controllable = true;
        private bool _isDashing;
        private bool _won;
        private bool _destroyed;


        public Dictionary<int, Platform> _platforms = new();
        private int _currentPlatform = 0;

        private void Start()
        {
            LevelManager.Instance.OnGameInit += InstanceOnOnGameInit;
        }

        private void InstanceOnOnGameInit(LevelSettings settings)
        {
            _won = false;
            _destroyed = false;
            _controllable = true;
            _isDashing = false;
            _currentPlatform = 0;
        }


        private void Update()
        {
            if (_won) return;
            if (_destroyed) return;
            if (_controllable)
            {
                if (Input.GetMouseButtonDown(0))
                    _isDashing = true;
                if (Input.GetMouseButtonUp(0))
                {
                    _isDashing = false;
                    rb.angularVelocity = Vector3.zero;
                }
            }

            if (_isDashing)
            {
                Dash();
                rb.AddTorque(
                    new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f),
                        Random.Range(-1f, 1f)) * 20f, ForceMode.Impulse);
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            rb.velocity = Vector3.up * bouncePower;
            if (_isDashing)
            {
                if (other.transform.parent.TryGetComponent(out Platform platform))
                {
                    if (platform.IsDamageTile(other.transform.gameObject))
                    {
                        DestroyMe();
                    }
                    else
                    {
                        _brickAudioController.PlayRandomClip();
                        platform.DestroyPlatform();
                    }
                }
                else
                {
                    ReachFinishLine();
                    transform.position = other.gameObject.transform.position;
                }
            }
            else
            {
                Instantiate(bumpingEffect, transform.position - new Vector3(0, transform.localScale.y / 2, 0),
                    Quaternion.identity);
                bumpingSource.Play();
                if (other.gameObject.CompareTag("Finish Line")) ReachFinishLine();
            }
        }

        private void Dash()
        {
            rb.velocity = Vector3.down * gravityPower;
        }

        private void ReachFinishLine()
        {
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