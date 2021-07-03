using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpawnDirection
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
    [SerializeField] private RoomTemplate roomTemplates;

    private RoomManager roomManager;

    private bool spawned;

    private int rand;

    public bool Spawned { get => spawned; set => spawned = value; }
    public SpawnDirection OpeningDirection { get => openingDirection; set => openingDirection = value; }

    void Start()
    {
        roomManager = RoomManager.instance;
        SpawnDungeon();
    }

    public void SpawnDungeon()
    {
        Invoke("SpawnRoom", 0.001f);
    }

    public void SpawnRoom()
    {
        if (spawned == false && CheckValidSpawnPoint())
        {
            switch (OpeningDirection)
            {
                case SpawnDirection.Bottomdoor:
                    rand = Random.Range(0, roomTemplates.Rooms.Length);
                    Instantiate(roomTemplates.Rooms[rand], transform.position, Quaternion.identity, roomManager.RoomsParent);
                    break;
                case SpawnDirection.Topdoor:
                    rand = Random.Range(0, roomTemplates.Rooms.Length);
                    Instantiate(roomTemplates.Rooms[rand], transform.position, Quaternion.identity, roomManager.RoomsParent);
                    break;
                case SpawnDirection.Leftdoor:
                    rand = Random.Range(0, roomTemplates.Rooms.Length);
                    Instantiate(roomTemplates.Rooms[rand], transform.position, Quaternion.identity, roomManager.RoomsParent);
                    break;
                case SpawnDirection.Rightdoor:
                    rand = Random.Range(0, roomTemplates.Rooms.Length);
                    Instantiate(roomTemplates.Rooms[rand], transform.position, Quaternion.identity, roomManager.RoomsParent);
                    break;
                default:
                    break;
            }
            spawned = true;
        }
    }

    private bool CheckValidSpawnPoint()
    {
        for (int i = 0; i < roomManager.Rooms.Count; i++)
        {
            if (transform.position == roomManager.Rooms[i].transform.position)
            {
                return false;
            }
        }
        return true;
    }
}
