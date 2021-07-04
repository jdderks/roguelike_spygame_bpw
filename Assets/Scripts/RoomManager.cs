using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private Transform roomsParent;
    [SerializeField] private Transform enemiesParent;

    [SerializeField] private GameObject treasureObject;
    [SerializeField] private GameObject enemy;

    [SerializeField] private List<Room> rooms = new List<Room>();

    [SerializeField] private List<RoomSpawnPoint> roomSpawnPointsInScene = new List<RoomSpawnPoint>();

    [SerializeField] private List<Enemy> enemies = new List<Enemy>();

    [SerializeField] private bool donePlacingRooms = false;

    [SerializeField] private GameObject startingRoomPrefab;
    [SerializeField] private GameObject startingRoom;

    [SerializeField] private Player player;

    private int roomsPlaced = 0;

    private bool setTreasure = false;


    private int amountOfEnemies = 2;

    private float waitTime = 1f;

    public List<Room> Rooms { get => rooms; set => rooms = value; }
    public List<Enemy> Enemies { get => enemies; set => enemies = value; }
    public bool DonePlacingRooms { get => donePlacingRooms; set => donePlacingRooms = value; }
    public int AmountOfEnemies { get => amountOfEnemies; set => amountOfEnemies = value; }
    public Transform RoomsParent { get => roomsParent; set => roomsParent = value; }
    public List<RoomSpawnPoint> RoomSpawnPointsInScene { get => roomSpawnPointsInScene; set => roomSpawnPointsInScene = value; }
    public int RoomsPlaced { get => roomsPlaced; set => roomsPlaced = value; }

    public static RoomManager instance;

    private void Awake()
    {
        if (instance == null || instance != this)
        {
            instance = this;
        }
    }

    public void UpdateRoomSpawnPointList()
    {
        RoomSpawnPointsInScene.Clear();
        RoomSpawnPointsInScene.AddRange(FindObjectsOfType<RoomSpawnPoint>());
    }

    private void Start()
    {
        CreateDungeon();
    }

    private void Update()
    {
        if (waitTime <= 0 && !donePlacingRooms)
        {
            donePlacingRooms = true;
            AstarPath.active.Scan();
            if (!setTreasure)
            {
                for (int i = 0; i < rooms.Count; i++)
                {
                    if (AmountOfEnemies > 0)
                    {
                        GameObject enObj = Instantiate(enemy, rooms[Random.Range(3, rooms.Count)].transform.position, Quaternion.identity, enemiesParent);
                        Enemies.Add(enObj.GetComponent<Enemy>());
                        AmountOfEnemies--;
                    }
                }
                if (!rooms[rooms.Count - 1].IsClosedRoom)
                {
                    Instantiate(treasureObject, rooms[rooms.Count - 1].transform.position, Quaternion.identity, rooms[rooms.Count - 1].transform);
                } else
                {
                    Instantiate(treasureObject, rooms[rooms.Count - 2].transform.position, Quaternion.identity, rooms[rooms.Count - 1].transform);
                }
                setTreasure = true;
            }
        }
        else
        {
            waitTime -= Time.deltaTime;
        }
    }

    public void AddRoom(Room point)
    {
        rooms.Add(point);
    }

    [ContextMenu("Recreate Dungeon")]
    public void RecreateDungeon()
    {
        player.transform.position = new Vector3(0, 0, 0);
        DestroyAllDungeonContent();
        CreateDungeon();
    }

    public void DestroyAllDungeonContent()
    {
        List<GameObject> roomsInDungeon = new List<GameObject>();
        for (int i = 0; i < roomsParent.childCount; i++)
        {
            roomsInDungeon.Add(roomsParent.GetChild(i).gameObject);
        }

        List<GameObject> enemiesInDungeon = new List<GameObject>();

        for (int i = 0; i < enemiesParent.childCount; i++)
        {
            enemiesInDungeon.Add(enemiesParent.GetChild(i).gameObject);
        }

        foreach (GameObject roomChild in roomsInDungeon)
        {
            Destroy(roomChild);
        }

        foreach (GameObject enemy in enemiesInDungeon)
        {
            Destroy(enemy);
        }

        enemies.Clear();
        enemiesInDungeon.Clear();

        rooms.Clear();
        roomsInDungeon.Clear();
    }

    public void CreateDungeon()
    {
        setTreasure = false;
        donePlacingRooms = false;
        waitTime = 1f;
        amountOfEnemies = 2;

        startingRoom = Instantiate(startingRoomPrefab, new Vector3(0, 0, 0), Quaternion.identity, roomsParent);

        if (startingRoom != null)
        {
            RoomSpawnPoint[] spawnPoints = startingRoom.GetComponentsInChildren<RoomSpawnPoint>();
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                if (spawnPoints[i].OpeningDirection != SpawnDirection.None)
                {
                    spawnPoints[i].SpawnDungeon();
                    spawnPoints[i].Spawned = false;
                }
            }
        }
    }

    public List<Transform> GetNearestRooms(Transform transform, float range)
    {
        List<Transform> nearestRooms = new List<Transform>();
        for (int i = 0; i < rooms.Count; i++)
        {
            if (Vector3.Distance(transform.position, rooms[i].transform.position) < range)
            {
                nearestRooms.Add(rooms[i].transform);
            }
        }

        return nearestRooms;
    }

    public bool CheckSpawnPoints(RoomSpawnPoint point)
    {
        for (int i = 0; i < roomSpawnPointsInScene.Count; i++)
        {
            if (point.transform.position == roomSpawnPointsInScene[i].transform.position && 
                point != roomSpawnPointsInScene[i])
            {
                Debug.Log(point.name + " shares a position with " + roomSpawnPointsInScene[i].name);
                return true;
            }
        }
        return false;
    }


        //if (!point.Spawned)
        //{
        //    for (int i = 0; i < roomSpawnPointsInScene.Count; i++)
        //    {
        //        if (!roomSpawnPointsInScene[i].Spawned &&
        //            roomSpawnPointsInScene[i] != point && 
        //            roomSpawnPointsInScene[i].transform.position == point.transform.position &&
        //            point.OpeningDirection != SpawnDirection.None)
        //        {
        //            Debug.Log(roomSpawnPointsInScene[i].name + "    " + point.name);
        //        }
        //    }
        //}
}
//if (!roomSpawnPointsInScene[i].Spawned)
//{
//    for (int j = 0; j < roomSpawnPointsInScene.Count; j++)
//    {
//        if (!roomSpawnPointsInScene[j].Spawned && roomSpawnPointsInScene[i] != roomSpawnPointsInScene[j])
//        {
//            Debug.Log("YEEEEEEEEEEEEEEEEEEEEEE");
//        }
//    }
//}