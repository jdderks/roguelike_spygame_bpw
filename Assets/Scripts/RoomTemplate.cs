using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Room Template", menuName = "ScriptableObjects/New room template")]

public class RoomTemplate : ScriptableObject
{
    [SerializeField] private GameObject[] rooms;
    [SerializeField] private GameObject closedRoom;


    public GameObject[] Rooms { get => rooms; set => rooms = value; }
    public GameObject ClosedRoom { get => closedRoom; set => closedRoom = value; }
}
