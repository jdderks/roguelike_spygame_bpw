using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField]
    private RoomManager roomManager;

    [SerializeField] RoomSpawnPoint[] spawnPoints;

    [SerializeField] private bool isClosedRoom = false;

    public bool IsClosedRoom { get => isClosedRoom; set => isClosedRoom = value; }

    void Start()
    {
        roomManager = GameObject.FindGameObjectWithTag("RoomManager").GetComponent<RoomManager>();
        roomManager.AddRoom(this);
    }
}
