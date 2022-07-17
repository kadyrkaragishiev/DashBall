using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace kadyrkaragishiev.Scripts
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField]
        private Platform platform;

        [SerializeField]
        private float distanceBetweenPlatforms = 0.35f;

        [SerializeField]
        private Transform centerPillar;

        [SerializeField]
        private Transform finishLine;

        [SerializeField]
        private float levelMinLenght = 10f;

        [SerializeField]
        private float levelMaxLenght = 20f;

        [SerializeField]
        private Material normalMaterial;

        [SerializeField]
        private Material damageMaterial;

        [SerializeField]
        private Transform commonPivot;

        [SerializeField]
        private Ball _ball;

        private ObjectPool<Platform> PlatformPool;

        private void Awake()
        {
            PlatformPool = new ObjectPool<Platform>(CreatePooledObject, OnTakeFromPool, OnReturnToPool, OnDestroyObject,
                false, 200, 100_000);
        }
        private void Start() => Initialize();
        private Platform CreatePooledObject()
        {
            Platform instance = Instantiate(platform, Vector3.zero, Quaternion.identity);
            instance.OnDisableEvent += ReturnObjectToPool;
            instance.gameObject.SetActive(false);
            return instance;
        }
        private void OnGUI()
        {
                GUI.Label(new Rect(10, 10, 200, 30), $"Total Pool Size: {PlatformPool.CountAll}");
                GUI.Label(new Rect(10, 30, 200, 30), $"Active Objects: {PlatformPool.CountActive}");
          }

        private void ReturnObjectToPool(Platform instance) => PlatformPool.Release(instance);

        private void OnTakeFromPool(Platform instance)
        {
            instance.gameObject.SetActive(true);
            instance.transform.SetParent(commonPivot, true);
        }

        private void OnReturnToPool(Platform instance) => instance.gameObject.SetActive(false);
        private void OnDestroyObject(Platform instance) => Destroy(instance.gameObject);

        private void Initialize()
        {
            var levelLenght = Random.Range(levelMinLenght, levelMaxLenght);
            var numberOfPlatforms = Mathf.CeilToInt(levelLenght / distanceBetweenPlatforms);
            levelLenght = numberOfPlatforms * distanceBetweenPlatforms;

            centerPillar.localScale = new Vector3(1, levelLenght + 6, 1);
            finishLine.position = new Vector3(0, -levelLenght - 5, 0);

            for (var i = 0; i < numberOfPlatforms; i++)
            {
                // var t = Instantiate(platform, new Vector3(0, -i * distanceBetweenPlatforms, 0), Quaternion.identity);
                var t = PlatformPool.Get(out Platform instance);
                instance.transform.position = new Vector3(0, -i * distanceBetweenPlatforms, 0);
                instance.transform.rotation = Quaternion.identity;
                var allTileIndecies = new List<int>(9) {0, 1, 2, 3, 4, 5, 6, 7, 8, 9};
                var damageTileIndecies = new List<int>();
            
                var damageTileCount = Random.Range(0, 5);
                for (var j = 0; j < damageTileCount; j++)
                {
                    var randomIndex = Random.Range(0, allTileIndecies.Count);
                    damageTileIndecies.Add(allTileIndecies[randomIndex]);
                    allTileIndecies.RemoveAt(randomIndex);
                }
            
                instance.Initialize(damageTileIndecies, normalMaterial, damageMaterial);
                // t.transform.parent = commonPivot;
                _ball._platforms.Add(i, instance);
            }
        }
        private void SpawnPlatform(Platform platform, Vector3 position, Quaternion rotation)
        {
            platform.transform.position = position;
            platform.transform.rotation = rotation;
            platform.gameObject.SetActive(true);
        }
    }
}