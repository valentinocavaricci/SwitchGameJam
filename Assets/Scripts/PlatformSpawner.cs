using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject platformPrefab;
    public float spawnInterval = 2f;
    public float minY = -2f;
    public float maxY = 2f;

    public float baseMoveSpeed = 3f;
    public float speedIncreasePerPoint = 0.15f;
    public float maxMoveSpeed = 6f;

    private float currentMoveSpeed;

    void Start()
    {
        currentMoveSpeed = baseMoveSpeed;
        InvokeRepeating(nameof(SpawnPlatform), 1f, spawnInterval);
    }

    void SpawnPlatform()
    {
        float randomY = Random.Range(minY, maxY);
        Vector3 spawnPosition = new Vector3(transform.position.x, randomY, 0f);

        GameObject newPlatform = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);

        PlatformMover mover = newPlatform.GetComponent<PlatformMover>();
        if (mover != null)
        {
            mover.moveSpeed = currentMoveSpeed;
        }
    }

    public void UpdateDifficulty(int score)
    {
        currentMoveSpeed = baseMoveSpeed + (score * speedIncreasePerPoint);

        if (currentMoveSpeed > maxMoveSpeed)
        {
            currentMoveSpeed = maxMoveSpeed;
        }
    }
}