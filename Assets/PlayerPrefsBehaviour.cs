#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class PlayerPrefsBehaviour : Editor
{
    [MenuItem("Edit/PlayerPrefs/Delete All")]
    static void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }

    [MenuItem("Edit/PlayerPrefs/SayAboutPlayerPrefs")]
    static void SayAboutKeys()
    {
        Debug.Log(PlayerPrefs.GetInt("LevelProgress"));
    }
}
#endif