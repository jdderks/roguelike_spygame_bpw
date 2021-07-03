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

    [SerializeField] private List<Enemy> enemies = new List<Enemy>();

    [SerializeField] private bool donePlacingRooms = false;

    [SerializeField] private GameObject startingRoomPrefab;
    [SerializeField] private GameObject startingRoom;

    [SerializeField] private Player player;

    private bool setTreasure = false;


    private int amountOfEnemies = 2;

    private float waitTime = 1f;

    public List<Room> Rooms { get => rooms; set => rooms = value; }
    public List<Enemy> Enemies { get => enemies; set => enemies = value; }
    public bool DonePlacingRooms { get => donePlacingRooms; set => donePlacingRooms = value; }
    public int AmountOfEnemies { get => amountOfEnemies; set => amountOfEnemies = value; }
    public Transform RoomsParent { get => roomsParent; set => roomsParent = value; }

    public static RoomManager instance;

    private void Awake()
    {
        if (instance == null || instance != this)
        {
            instance = this;
        }
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

                Instantiate(treasureObject, rooms[rooms.Count - 1].transform.position, Quaternion.identity, rooms[rooms.Count - 1].transform);
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
            DestroyImmediate(roomChild);
        }

        foreach (GameObject enemy in enemiesInDungeon)
        {
            DestroyImmediate(enemy);
        }

        enemies.Clear();
        enemiesInDungeon.Clear();

        rooms.Clear();
        roomsInDungeon.Clear();

        //for (int i = 0; i < rooms.Count; i++)
        //{
        //    if (rooms[i].gameObject.name != "StartRoom")
        //    {
        //        Destroy(rooms[i].gameObject);
        //    }
        //}
        //for (int i = 0; i < enemies.Count; i++)
        //{
        //    Destroy(enemies[i].gameObject);
        //}
        //rooms.Clear();
        //enemies.Clear();
    }

    public void CreateDungeon()
    {
        setTreasure = false;
        donePlacingRooms = false;
        waitTime = 1f;
        amountOfEnemies = 2;

        startingRoom = Instantiate(startingRoomPrefab, new Vector3(0,0,0), Quaternion.identity, roomsParent);

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
}
