using DG.Tweening;
using kadyrkaragishiev.LevelingSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace kadyrkaragishiev.Scripts
{
    public class ScoreBarMenu : MonoBehaviour
    {
        [SerializeField]
        private Slider _scoreBar;

        [SerializeField]
        private TextMeshProUGUI _scoreText;

        [SerializeField]
        private TextMeshProUGUI _currentLevelText;

        private int _lastLevelLenght;

        private void Awake() =>
            LevelManager.Instance.OnGameInit += settings =>
            {
                _currentLevelText.text = settings.name;
                _lastLevelLenght = Mathf.CeilToInt(settings.LevelLength / settings.DistanceBetweenPlatforms);
            };

        public void SetScore(float score, int levelLenght)
        {
            Debug.Log(score + "  " + levelLenght);
            DOTween.To(() => _scoreBar.value, x => _scoreBar.value = x,
                score / _lastLevelLenght,
                0.2f);
            _scoreText.text = "Score " + score;
        }
    }
}
