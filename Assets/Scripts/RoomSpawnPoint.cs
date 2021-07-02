using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum SpawnDirection
{
    Bottomdoor = 1,
    Topdoor = 2,
    Leftdoor = 3,
    Rightdoor = 4,
    None = 5
}

public class RoomSpawnPoint : MonoBehaviour
{
    [SerializeField] private SpawnDirection openingDirection;
    [SerializeField] private GameObject enemy;
    [SerializeField] private RoomTemplate roomTemplates;

    private bool spawned;

    private int rand;

    public bool Spawned { get => spawned; set => spawned = value; }

    void Start()
    {
        Invoke("SpawnRoom", 0.001f);
    }

    void SpawnRoom()
    {
        if (spawned == false)
        {
            switch (openingDirection)
            {
                case SpawnDirection.Bottomdoor:
                    rand = Random.Range(0, roomTemplates.Rooms.Length);
                    Instantiate(roomTemplates.Rooms[rand], transform.position, Quaternion.identity);
                    break;
                case SpawnDirection.Topdoor:
                    rand = Random.Range(0, roomTemplates.Rooms.Length);
                    Instantiate(roomTemplates.Rooms[rand], transform.position, Quaternion.identity);
                    break;
                case SpawnDirection.Leftdoor:
                    rand = Random.Range(0, roomTemplates.Rooms.Length);
                    Instantiate(roomTemplates.Rooms[rand], transform.position, Quaternion.identity);
                    break;
                case SpawnDirection.Rightdoor:
                    rand = Random.Range(0, roomTemplates.Rooms.Length);
                    Instantiate(roomTemplates.Rooms[rand], transform.position, Quaternion.identity);
                    break;
                default:
                    break;
            }
            if (gameObject.name != "StartRoom")
            {
                if (Random.Range(1, 100) > 70)
                {
                    Instantiate(enemy, transform.position, Quaternion.identity);
                }
            }
            spawned = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("RoomSpawnPoint"))
        {
            if (other.GetComponent<RoomSpawnPoint>().spawned == false && spawned == false)
            {
                Instantiate(roomTemplates.ClosedRoom, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            spawned = true;
        }
    }
}
