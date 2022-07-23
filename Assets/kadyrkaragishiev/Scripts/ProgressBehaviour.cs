using System;
using UnityEngine;

namespace kadyrkaragishiev.Scripts
{
    public class ProgressBehaviour : MonoBehaviour
    {
        public static ProgressBehaviour Instance;
        public event Action<int> OnProgressChanged;

        public int Progress
        {
            get => _progress;
            set
            {
                _progress = value;
                PlayerPrefs.SetInt("LevelProgress", _progress);
                OnProgressChanged?.Invoke(_progress);
            }
        }

        private int _progress;

        private void Awake()
        {
            if (Instance == null) Instance = this;
        }

        private void Start() => Progress = PlayerPrefs.GetInt("LevelProgress", 0);
    }
}