using UnityEngine;
#if UNITY_IOS
    using TapticPlugin;
#endif

namespace kadyrkaragishiev.Scripts
{
    public static class Haptic
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        public static AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

        public static AndroidJavaObject CurrentActivity =
            UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        public static AndroidJavaObject Vibrator =
            CurrentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");

#else
        public static AndroidJavaClass UnityPlayer;
        public static AndroidJavaObject CurrentActivity;
        public static AndroidJavaObject Vibrator;
#endif
        //vibration works only with long argument
        public static void Vibrate(long milliseconds)
        {
#if UNITY_ANDROID || UNITY_EDITOR
            if (IsAndroid())
                Vibrator.Call("vibrate", milliseconds);
            else
            {
                Handheld.Vibrate();
            }
#elif UNITY_IOS
            TapticManager.Impact(ImpactFeedback.Light);
#endif
        }

        private static bool IsAndroid()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            return true;
#else
            return false;
#endif
        }
    }
}