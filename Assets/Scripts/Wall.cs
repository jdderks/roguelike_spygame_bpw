using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] private bool isDiscovered = false;
    [SerializeField] private GameObject discoveredUI = null;

    public bool IsDiscovered { get => isDiscovered; set => isDiscovered = value; }

    public void DiscoverWall()
    {
        IsDiscovered = true;
        discoveredUI.SetActive(IsDiscovered);
    }

    public void UnDiscoverWall()
    {
        IsDiscovered = false;
        discoveredUI.SetActive(IsDiscovered);
    }

}
