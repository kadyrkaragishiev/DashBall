using System.Collections.Generic;
using kadyrkaragishiev.Scripts;
using kadyrkaragishiev.UI;
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
        private AudioSource _audioSource;

        [SerializeField]
        private Transform CanvasContent;

        private LevelSettings _lastSettings;
        private List<Button> _buttons = new();

        private void Start()
        {
            InitCanvasContent();
            ProgressBehaviour.Instance.OnProgressChanged += InstanceOnOnProgressChanged;
            InstanceOnOnProgressChanged(PlayerPrefs.GetInt("LevelProgress"));
        }

        private void InstanceOnOnProgressChanged(int progress)
        {
            for (var i = 0; i < _buttons.Count; i++)
            {
                LevelManager.Instance.settingsList[i].isUnlocked = i <= progress;
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
                    _audioSource.Play();
                });
                btn.GetComponent<Button>().interactable = LevelManager.Instance.settingsList[i].isUnlocked;
                btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (i+1).ToString();
            }
        }
    }
}