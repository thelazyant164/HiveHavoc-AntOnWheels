using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject objectToInstantiate; // The object to be instantiated
    [SerializeField]
    private Transform spawnPoint; // The position where the object will be instantiated
    [SerializeField]
    private float spawnInterval = 5f; // Time interval between spawns

    private void Start()
    {
        // Start the spawning coroutine
        StartCoroutine(SpawnObjectRoutine());
    }

    private IEnumerator SpawnObjectRoutine()
    {
        while (true)
        {
            Vector3 spawnPosition = new Vector3(spawnPoint.position.x, 0, spawnPoint.position.z);
            
            // Instantiate the object at the specified spawn point
            Instantiate(objectToInstantiate, spawnPosition, Quaternion.identity);
            
            // Wait for the specified interval before spawning the next object
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
