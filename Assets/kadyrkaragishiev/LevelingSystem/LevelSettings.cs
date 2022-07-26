using UnityEngine;

namespace kadyrkaragishiev.LevelingSystem
{
    [CreateAssetMenu(fileName = "Level", menuName = "Leveling System/Level", order = 1)]
    public class LevelSettings : ScriptableObject
    {
        public float DistanceBetweenPlatforms;
        public int LevelLength;
        public float WrongPlatformsChance;
        public float PlatformsSpeed;
        public bool isUnlocked;
        
        [ContextMenu("SetRandomLevel")]
        public void SetRandomLevel()
        {
            DistanceBetweenPlatforms = Mathf.Round(Random.Range(0.9f, 1.5f)*100)/100;
            LevelLength = Random.Range(40, 60);
            WrongPlatformsChance = Mathf.Round(Random.Range(0.4f, 0.7f));
            PlatformsSpeed = Mathf.Round(Random.Range(30f, 40f));
        }
    }
}
