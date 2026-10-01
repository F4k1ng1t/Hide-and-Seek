using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int currentRoomIndex;
    public EnemyManager manager;
    public float moveThreshold;
    bool inOffice = false;
    bool enemyVisible = false;
    int visibilityStacks = 0;
    int moveFrames = 0;
    public void Die()
    {
        Destroy(gameObject);
    }
    void TryMove()
    {
        float moveChance = Random.value;
        if (moveChance >= moveThreshold)
        {
            if (!inOffice)
            {
                ForwardOrBack();
            }
            else
            {
                Jumpscare();
            }
        }

    }
    void ForwardOrBack()
    {
        float forwardOrBack = Random.value;
        if (forwardOrBack >= 0.5f)
        {
            currentRoomIndex += 2;
            if (currentRoomIndex > manager.rooms.Count - 1)
            {
                MoveToOffice();
                return;
            }


        }
        else
        {
            currentRoomIndex -= 1;
        }

        UpdateEnemyPosition();

    }
    void Jumpscare()
    {
        //Jumpscare functionality
        Debug.Log("GRAHHHHH");
    }
    void UpdateEnemyPosition()
    {
        currentRoomIndex = Mathf.Clamp(currentRoomIndex, 0, manager.rooms.Count - 1);
        GameObject room = manager.rooms[currentRoomIndex];
        float randomX = room.transform.position.x + Random.Range(-manager.roomRadius, manager.roomRadius);
        float randomZ = room.transform.position.z + Random.Range(-manager.roomRadius, manager.roomRadius);

        gameObject.transform.position = new Vector3(randomX, gameObject.transform.position.y, randomZ);
    }
    void MoveToOffice()
    {
        inOffice = true;
    }
    void Start()
    {
        
    }
    void Update()
    {
        moveFrames++;
        if(moveFrames * Time.deltaTime >= 30f)
        {
            TryMove();
            moveFrames = 0;
        }
    }
}
