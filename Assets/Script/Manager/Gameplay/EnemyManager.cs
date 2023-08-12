using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Unnamed.RacingGame.Enemy
{
    public sealed class EnemyManager : Singleton<EnemyManager>
    {
        private HashSet<EnemySpawner> spawners = new();

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void OnEnable()
        {
            foreach (EnemySpawner spawner in spawners)
            {
                spawner.gameObject.SetActive(true);
            }
        }

        private void OnDisable()
        {
            foreach (EnemySpawner spawner in spawners)
            {
                spawner.gameObject.SetActive(false);
            }
        }

        internal void RegisterSpawner(EnemySpawner spawner) => spawners.Add(spawner);

        internal void UnregisterSpawner(EnemySpawner spawner) => spawners.Remove(spawner);
    }
}
