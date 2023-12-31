using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Enemy
{
    public sealed class EnemySpawner : MonoBehaviour, IDestructible<EnemySpawner>
    {
        [SerializeField]
        private Enemy enemyPrefab;
        private Transform spawnPosition;
        private Vector3 spawnPos;

        [SerializeField]
        private int maxCapacity = 3;

        [SerializeField]
        private float spawnCooldown = 5f;

        [SerializeField]
        private LayerMask destroyedBy;
        public LayerMask DestroyedBy => destroyedBy;

        private Coroutine spawn;
        private EnemyManager enemyManager;
        private HashSet<Enemy> spawned = new();
        public event EventHandler<EnemySpawner> OnDestroy;

        private void Awake()
        {
            spawnPosition = GetComponent<Transform>();
            SampleSpawnPosition(); // happens once during awake -> assume spawn position does not change throughout game
        }

        private void Start()
        {
            enemyManager = EnemyManager.Instance;
            enemyManager.RegisterSpawner(this);
            spawn = StartCoroutine(Spawn());
        }

        private void OnEnable()
        {
            if (enemyManager != null)
                spawn = StartCoroutine(Spawn());
        }

        private void OnDisable()
        {
            if (spawn != null)
                StopCoroutine(spawn);
        }

        public void Destroy()
        {
            OnDestroy?.Invoke(this, this);
            Destroy(gameObject);
        }

        private void SampleSpawnPosition()
        {
            if (
                NavMesh.SamplePosition(
                    spawnPosition.position,
                    out NavMeshHit closestHit,
                    spawnPosition.position.y,
                    NavMesh.AllAreas
                )
            )
            {
                spawnPos = closestHit.position;
            }
            else
            {
                Debug.LogWarning("Cannot sample NavMesh; set default spawn location y to 0");
                spawnPos = new Vector3(spawnPosition.position.x, 0, spawnPosition.position.z);
            }
        }

        private Enemy SpawnSingle()
        {
            Enemy enemy = Instantiate(enemyPrefab.gameObject, spawnPos, Quaternion.identity)
                .GetComponent<Enemy>();
            enemy.transform.SetParent(enemyManager.transform, true);
            spawned.Add(enemy);
            enemy.OnDeath += (object sender, EventArgs e) => spawned.Remove(enemy);
            return enemy;
        }

        private IEnumerator Spawn()
        {
            while (true)
            {
                if (spawned.Count < maxCapacity)
                {
                    spawned.Add(SpawnSingle());
                    yield return new WaitForSeconds(spawnCooldown);
                }
                yield return null;
            }
        }
    }
}
