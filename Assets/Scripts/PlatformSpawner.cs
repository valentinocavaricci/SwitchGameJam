using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject platformPrefab;
    public float spawnInterval = 2f;
    public float minY = -2f;
    public float maxY = 2f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnPlatform), 1f, spawnInterval);
    }

    void SpawnPlatform()
    {
        float randomY = Random.Range(minY, maxY);
        Vector3 spawnPosition = new Vector3(transform.position.x, randomY, 0f);

        Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
    }
}