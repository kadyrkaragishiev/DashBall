using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace kadyrkaragishiev.Scripts
{
    public class LogoLoader : MonoBehaviour
    {
        [SerializeField]
        private string videoName = "ninsar_logo.mp4";
        private void Start() => StartCoroutine(PlayVideo());

        private IEnumerator PlayVideo()
        {
            Handheld.PlayFullScreenMovie(videoName, Color.black, FullScreenMovieControlMode.CancelOnInput);
            yield return new WaitForEndOfFrame();
            SceneManager.LoadScene(1);
        }
    }
}
