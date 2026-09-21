using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int currentRoomIndex;
    public EnemyManager manager;
    public float moveThreshold;
    bool canMove = true;
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
            ForwardOrBack();
        }
    }
    void ForwardOrBack()
    {
        float forwardOrBack = Random.value;
        if (forwardOrBack >= 0.5f && canMove)
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
        
    }
    void MoveToOffice()
    {
        canMove = false;
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
