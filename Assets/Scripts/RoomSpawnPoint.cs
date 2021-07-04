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
    [SerializeField] private bool spawned;

    private RoomManager roomManager;


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
        roomManager.UpdateRoomSpawnPointList();

        if (spawned == false && CheckValidSpawnPoint())
        {
            GameObject room = null;
            switch (OpeningDirection)
            {
                case SpawnDirection.Bottomdoor:
                    rand = Random.Range(0, roomTemplates.Rooms.Length);
                    room = Instantiate(roomTemplates.Rooms[rand], transform.position, Quaternion.identity, roomManager.RoomsParent);
                    room.name = "Room" + roomManager.RoomsPlaced;
                    roomManager.RoomsPlaced++;
                    break;
                case SpawnDirection.Topdoor:
                    rand = Random.Range(0, roomTemplates.Rooms.Length);
                    room = Instantiate(roomTemplates.Rooms[rand], transform.position, Quaternion.identity, roomManager.RoomsParent);
                    room.name = "Room" + roomManager.RoomsPlaced;
                    roomManager.RoomsPlaced++;
                    break;
                case SpawnDirection.Leftdoor:
                    rand = Random.Range(0, roomTemplates.Rooms.Length);
                    room = Instantiate(roomTemplates.Rooms[rand], transform.position, Quaternion.identity, roomManager.RoomsParent);
                    room.name = "Room" + roomManager.RoomsPlaced;
                    roomManager.RoomsPlaced++;
                    break;
                case SpawnDirection.Rightdoor:
                    rand = Random.Range(0, roomTemplates.Rooms.Length);
                    room = Instantiate(roomTemplates.Rooms[rand], transform.position, Quaternion.identity, roomManager.RoomsParent);
                    room.name = "Room" + roomManager.RoomsPlaced;
                    roomManager.RoomsPlaced++;
                    break;
                default:
                    break;
            }

            if (roomManager.CheckSpawnPoints(this))
            {
                if (room != null)
                {
                    Destroy(room);
                }
                GameObject closedRoom = Instantiate(roomTemplates.ClosedRoom, transform.position, Quaternion.identity);
                closedRoom.GetComponent<Room>().IsClosedRoom = true;
            }
            else
            {
                spawned = true;
            }

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
