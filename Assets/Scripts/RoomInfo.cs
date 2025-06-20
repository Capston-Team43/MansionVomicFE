using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomInfo : MonoBehaviour
{
    public string floorName;

    void Awake()
    {
        if (string.IsNullOrEmpty(floorName) && transform.parent != null)
        {
            floorName = transform.parent.name;
        }
    }
}