using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    public GameObject objectToSpawn; // The object to spawn
    public Transform[] spawnPoints; // Array of spawn points

    void Start()
    {
        SpawnAtRandomLocation();
    }

    void SpawnAtRandomLocation()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogWarning("No spawn points set!");
            return;
        }

        // Select a random spawn point
        int randomIndex = Random.Range(0, spawnPoints.Length);

        // Instantiate the object at the selected spawn point's position and rotation
        Instantiate(objectToSpawn, spawnPoints[randomIndex].position, spawnPoints[randomIndex].rotation);
    }
}