using System;
using System.Collections.Generic;
using UnityEngine;

namespace kadyrkaragishiev.Scripts
{
    public class OnBoardingBehaviour : MonoBehaviour
    {
        public static OnBoardingBehaviour Instance;
        [SerializeField]
        private List<GameObject> onBoardings;

        private void Awake()
        {
            if (Instance == null) Instance = this;
        }

        public void CallOnBoarding(string onBoardingName)
        {
            foreach (var t in onBoardings) t.SetActive(t.name == onBoardingName);
        }
    }
}
