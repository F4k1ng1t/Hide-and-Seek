using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    public List<GameObject> rooms = new List<GameObject>();
    public List<Enemy> enemies = new List<Enemy>();
    public CamController camController;
    public float enemyHalfHeight = 1f;
    public float roomRadius = 5f;
    private int currentRoomIndex = 0;
    void Start()
    {
        Spawn();
        Spawn();
        Spawn();
        Spawn();
        Spawn();
    }
    Vector3 RandomizeEnemyPosition()
    {
        currentRoomIndex = Random.Range(0, rooms.Count);
        GameObject room = rooms[currentRoomIndex];
        float randomX = room.transform.position.x + Random.Range(-roomRadius, roomRadius);
        float randomZ = room.transform.position.z + Random.Range(-roomRadius, roomRadius);
        float Y = room.transform.position.y + enemyHalfHeight;
        return new Vector3(randomX, Y, randomZ);
    }
    Quaternion LookAtCamera(Vector3 position)
    {
        if (camController == null)
        {
            return Quaternion.identity; // Default no-rotation fallback
        }
        if (rooms.Count == camController.cameraList.Count)
        {
            Vector3 direction = camController.cameraList[currentRoomIndex].transform.position - position;
            // direction.y = 0; 
            return Quaternion.LookRotation(direction);
        }
        


        // Return the rotation pointing toward that direction
        return new Quaternion(0, 0, 0, 0);
    }
    void Spawn()
    {

        Vector3 randomizedPosition = RandomizeEnemyPosition();
        Quaternion look = LookAtCamera(randomizedPosition);
        Enemy enemy = Instantiate(enemyPrefab, randomizedPosition, look).GetComponent<Enemy>();
        enemies.Add(enemy);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    
}
