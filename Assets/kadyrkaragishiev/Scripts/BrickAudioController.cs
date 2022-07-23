using System.Collections.Generic;
using UnityEngine;

namespace kadyrkaragishiev.Scripts
{
    public class BrickAudioController : MonoBehaviour
    {
        [SerializeField]
        private AudioSource _audioSource;
        
        [SerializeField]
        private List<AudioClip> _audioClips;
        
        public void PlayRandomClip()
        {
            _audioSource.clip = _audioClips[Random.Range(0, _audioClips.Count)];
            _audioSource.Play();
        }
    }
}
