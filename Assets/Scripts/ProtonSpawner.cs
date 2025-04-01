using UnityEngine;

public class ProtonSpawner : MonoBehaviour
{
    public GameObject protonPrefab;
    public int maxProtons = 5;
    public Transform spawnArea;
    public float spawnInterval = 1f; // Time between spawn attempts

    private float lastSpawnTime;
    private int currentProtonCount = 0;

    void Update()
    {
        // Only attempt to spawn if enough time has passed and max protons not reached
        if (Time.time - lastSpawnTime >= spawnInterval && currentProtonCount < maxProtons)
        {
            SpawnProton();
        }
    }

    void SpawnProton()
    {
        Vector3 randomPosition = spawnArea.position + new Vector3(2, 1, Random.Range(-1f, 1f));

        // Instantiate the proton
        GameObject spawnedProton = Instantiate(protonPrefab, randomPosition, Quaternion.identity);

        // Optional: Add a script to track proton destruction
        ProtonLifecycle lifecycleScript = spawnedProton.AddComponent<ProtonLifecycle>();
        lifecycleScript.spawner = this;

        // Update spawn tracking
        currentProtonCount++;
        lastSpawnTime = Time.time;
    }

    // Method to be called when a proton is destroyed
    public void OnProtonDestroyed()
    {
        currentProtonCount--;
    }
}

// Helper script to track proton lifecycle
public class ProtonLifecycle : MonoBehaviour
{
    public ProtonSpawner spawner;

    void OnDestroy()
    {
        if (spawner != null)
        {
            spawner.OnProtonDestroyed();
        }
    }
}