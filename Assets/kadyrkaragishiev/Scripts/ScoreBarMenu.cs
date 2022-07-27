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

        private void Start()
        {
            LevelManager.Instance.OnGameInit += delegate(LevelSettings settings)
            {
                _currentLevelText.text = settings.name;
            };
        }
        public void SetScore(float score, int levelLenght)
        {
            _scoreBar.value = score/levelLenght;
            _scoreText.text = "Score "+ score;
        }
    }
}
