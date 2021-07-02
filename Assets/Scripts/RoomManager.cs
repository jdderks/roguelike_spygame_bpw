using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private GameObject treasureObject;

    [SerializeField] private List<Room> rooms = new List<Room>();

    [SerializeField] private bool donePlacingRooms = false;
    private bool setTreasure = false;

    private float waitTime = 1f;

    public List<Room> Rooms { get => rooms; set => rooms = value; }
    public bool DonePlacingRooms { get => donePlacingRooms; set => donePlacingRooms = value; }


    public static RoomManager instance;

    private void Awake()
    {
        if (instance == null || instance != this)
        {
            instance = this;
        }
    }

    private void Update()
    {
        if (waitTime <= 0 && !donePlacingRooms)
        {
            donePlacingRooms = true;
            AstarPath.active.Scan();
            if (!setTreasure)
            {
                Instantiate(treasureObject, rooms[rooms.Count-1].transform.position, Quaternion.identity);
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
