using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Com.Unnamed.RacingGame.Enemy
{
    public sealed class EnemySpawner : MonoBehaviour
    {
        [SerializeField]
        private Enemy enemyPrefab;
        private Transform spawnPosition;
        private Vector3 spawnPos;

        [SerializeField]
        private int maxCapacity = 3;

        [SerializeField]
        private float spawnCooldown = 5f;

        private Coroutine spawn;
        private EnemyManager enemyManager;
        private List<Enemy> spawned = new();

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

        private void OnDestroy() => enemyManager?.UnregisterSpawner(this);

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
