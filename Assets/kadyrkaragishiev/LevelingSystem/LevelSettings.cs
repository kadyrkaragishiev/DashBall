using UnityEngine;

namespace kadyrkaragishiev.LevelingSystem
{
    [CreateAssetMenu(fileName = "Level", menuName = "Leveling System/Level", order = 1)]
    public abstract class LevelSettings : ScriptableObject
    {
        public float DistanceBetweenPlatforms;
        public int LevelLength;
        public float WrongPlatformsChance;
        public float PlatformsSpeed;
    }
}
