using System;
using System.Collections.Generic;
using kadyrkaragishiev.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace kadyrkaragishiev.LevelingSystem
{
    public class LevelGridInitializer : MonoBehaviour
    {
        [SerializeField]
        private GameObject _buttonPrefab;

        [SerializeField]
        private Transform CanvasContent;

        private LevelSettings _lastSettings;
        private List<Button> _buttons = new List<Button>();

        private void Start()
        {
            InitCanvasContent();
            ProgressBehaviour.Instance.OnProgressChanged += InstanceOnOnProgressChanged;
            InstanceOnOnProgressChanged(PlayerPrefs.GetInt("LevelProgress"));
        }

        private void InstanceOnOnProgressChanged(int progress)
        {
            Debug.Log(ProgressBehaviour.Instance.Progress);
            for (var i = 0; i < _buttons.Count; i++)
            {
                LevelManager.Instance.settingsList[i].isUnlocked = i <= progress;
                Debug.Log(LevelManager.Instance.settingsList[i].isUnlocked);
                _buttons[i].interactable = LevelManager.Instance.settingsList[i].isUnlocked;
            }
        }

        private void InitCanvasContent()
        {
            for (int i = 0; i < LevelManager.Instance.settingsList.Count; i++)
            {
                var btn = Instantiate(_buttonPrefab, Vector3.zero, Quaternion.identity);
                btn.transform.parent = CanvasContent;
                btn.transform.localScale = new Vector3(1, 1, 1);
                var i1 = i;
                _buttons.Add(btn.GetComponent<Button>());
                btn.GetComponent<Button>().onClick.AddListener(() =>
                {
                    LevelManager.Instance.InitLevel(LevelManager.Instance.settingsList[i1]);
                    OnBoardingBehaviour.Instance.CallOnBoarding("");
                });
                btn.GetComponent<Button>().interactable = LevelManager.Instance.settingsList[i].isUnlocked;
                btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = i.ToString();
            }
        }
    }
}