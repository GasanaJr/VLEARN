using UnityEngine;

public class ProtonSpawner : MonoBehaviour
{
    public GameObject protonPrefab;
    public GameObject neutronPrefab;
    public GameObject electronPrefab;
    public int maxProtons = 5;
    public int maxNeutrons = 5;
    public int maxElectrons = 5;
    public Transform protonSpawnArea;
    public Transform neutronSpawnArea;
    public Transform electronSpawnArea;
    public float spawnInterval = 1f;
    public Nucleus nucleusReference;
    private int currentProtonCount = 0;
    private int currentNeutronCount = 0;
    private int currentElectronCount = 0;

    public int respawnThreshold = 2;

    void Start()
    {
        for (int i = 0; i < maxProtons; i++)
        {
            SpawnProton();
        }

        for (int i = 0; i < maxNeutrons; i++)
        {
            SpawnNeutron();
        }

        for (int i = 0; i < maxElectrons; i++)
        {
            SpawnElectron();
        }

        if (nucleusReference == null)
        {
            nucleusReference = FindObjectOfType<Nucleus>();
        }
    }

    void Update()
    {
        if (nucleusReference == null) return;
        int freeProtons = currentProtonCount - nucleusReference.attachedProtons.Count;
        int freeNeutrons = currentNeutronCount - nucleusReference.attachedNeutrons.Count;
        int freeElectrons = currentElectronCount - nucleusReference.attachedElectrons.Count;
        if (freeProtons < respawnThreshold)
        {
            SpawnProton();
        }
        if (
            freeNeutrons < respawnThreshold)
        {
            SpawnNeutron();
        }

        if (
            freeElectrons < respawnThreshold)
        {
            SpawnElectron();
        }
    }

    void SpawnProton()
    {
        Vector3 randomPosition = protonSpawnArea.position +  new Vector3(2, 1, Random.Range(-1f, 1f));
        GameObject spawnedProton = Instantiate(protonPrefab, randomPosition, Quaternion.identity);
        currentProtonCount++;
    }

    void SpawnNeutron()
    {
        Vector3 randomPosition = neutronSpawnArea.position +  new Vector3(2, 1, Random.Range(-1f, 1f));
        GameObject spawnedNeutron = Instantiate(neutronPrefab, randomPosition, Quaternion.identity);
        currentNeutronCount++;
    }

    void SpawnElectron()
    {
        Vector3 randomPosition = electronSpawnArea.position +  new Vector3(2, 1, Random.Range(-1f, 1f));
        GameObject spawnedElectron = Instantiate(electronPrefab, randomPosition, Quaternion.identity);
        currentElectronCount++;
    }
}

public enum ParticleType
{
    Proton,
    Neutron,
    Electron
}