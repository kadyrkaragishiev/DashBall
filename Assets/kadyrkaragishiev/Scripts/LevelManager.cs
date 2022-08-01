using System;
using System.Collections.Generic;
using System.Dynamic;
using kadyrkaragishiev.LevelingSystem;
using kadyrkaragishiev.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace kadyrkaragishiev.Scripts
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance;
        public event Action<LevelSettings> OnGameInit;

        public List<LevelSettings> settingsList;
        public LevelSettings _lastSettings;

        #region MaterialsForLevels

        [SerializeField]
        private Platform platform;

        [SerializeField]
        private Transform centerPillar;

        [SerializeField]
        private Transform finishLine;

        [SerializeField]
        private Material normalMaterial;

        [SerializeField]
        private Material damageMaterial;

        [SerializeField]
        private Transform commonPivot;

        #endregion

        #region LevelingSystem

        public float platformSpeed;

        [SerializeField]
        private float distanceBetweenPlatforms = 0.35f;

        [SerializeField]
        private int LevelLength = 10;

        private float _wrongPlatformChnace;

        #endregion

        [SerializeField]
        private Ball _ball;

        [SerializeField]
        private TextMeshProUGUI commonScoreText;

        private List<Platform> _platformList = new();

        private void Awake()
        {
            if (Instance == null) Instance = this;
            Application.targetFrameRate = 120;
        }

        private void Start()
        {
            var t = 0;
            for (var i = 0; i < ProgressBehaviour.Instance.Progress; i++)
                t += Mathf.CeilToInt(settingsList[i].LevelLength / settingsList[i].DistanceBetweenPlatforms);
            commonScoreText.text = $"{t}";
        }

        public void StartGame()
        {
            if (ProgressBehaviour.Instance.Progress >= settingsList.Count)
                ProgressBehaviour.Instance.Progress = settingsList.Count - 1;
            InitLevel(settingsList[ProgressBehaviour.Instance.Progress]);
        }

        public void InitLevel(LevelSettings settings)
        {
            if (settings == null)
                return;

            #region SettingLevel

            _lastSettings = settings;
            distanceBetweenPlatforms = settings.DistanceBetweenPlatforms;
            LevelLength = settings.LevelLength;
            _wrongPlatformChnace = settings.WrongPlatformsChance;
            platformSpeed = settings.PlatformsSpeed;

            #endregion

            #region CleaningLevel

            if (_platformList.Count > 0)
                foreach (Platform platform in _platformList)
                    if (platform != null)
                        Destroy(platform.gameObject);
            _platformList.Clear();
            _ball.gameObject.SetActive(true);
            _ball._platforms.Clear();

            #endregion

            Debug.Log("set settings");

            _ball.transform.position = new Vector3(0, 1, -1.15f);
            Initialize();
            OnGameInit?.Invoke(settings);
        }


        public void RestartLevel()
        {
            Instance.InitLevel(_lastSettings);
            OnBoardingBehaviour.Instance.CallOnBoarding("");
        }

        public void MoveToTheNextLevel()
        {
            for (var i = 0; i < settingsList.Count; i++)
            {
                if (settingsList[i] == _lastSettings)
                {
                    Debug.Log(settingsList[i].name + "   " + _lastSettings.name);
                    if (i + 1 < settingsList.Count)
                    {
                        InitLevel(settingsList[i + 1]);
                        OnBoardingBehaviour.Instance.CallOnBoarding("");
                        return;
                    }
                }
            }
        }


        private void Initialize()
        {
            var numberOfPlatforms = Mathf.CeilToInt(LevelLength / distanceBetweenPlatforms);
            LevelLength = (int) (numberOfPlatforms * distanceBetweenPlatforms);
            centerPillar.localScale = new Vector3(1, LevelLength + 6, 1);
            finishLine.position = new Vector3(0, -LevelLength - 5, 0);

            for (var i = 0; i < numberOfPlatforms; i++)
            {
                Platform instance = Instantiate(platform, centerPillar, true);
                instance.transform.position = new Vector3(0, -i * distanceBetweenPlatforms, 0);
                instance.transform.rotation = Quaternion.identity;
                var allTileIndecies = new List<int>(9)
                {
                    0,
                    1,
                    2,
                    3,
                    4,
                    5,
                    6,
                    7,
                    8,
                    9
                };
                var damageTileIndecies = new List<int>();

                var damageTileCount = Random.Range(0, 5);
                for (var j = 0; j < damageTileCount; j++)
                {
                    if (Random.Range(0, 0.95f) < _wrongPlatformChnace)
                    {
                        var randomIndex = Random.Range(0, allTileIndecies.Count);
                        damageTileIndecies.Add(allTileIndecies[randomIndex]);
                        allTileIndecies.RemoveAt(randomIndex);
                    }
                }

                instance.Initialize(damageTileIndecies, normalMaterial, damageMaterial);
                _platformList.Add(instance);
                _ball._platforms.Add(i, instance);
            }

            Debug.Log(numberOfPlatforms);
        }
    }
}
