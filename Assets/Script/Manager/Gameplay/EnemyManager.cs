using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Enemy
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
            spawners = spawners.Where(spawner => spawner != null).ToHashSet();
            foreach (EnemySpawner spawner in spawners)
            {
                spawner.gameObject.SetActive(false);
            }
        }

        internal void RegisterSpawner(EnemySpawner spawner)
        {
            spawners.Add(spawner);
            spawner.OnDestroy += (object sender, EnemySpawner destroyedSpawner) => spawners.Remove(spawner);
        }
    }
}
