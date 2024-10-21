using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMover : MonoBehaviour
{
    [SerializeField] private Transform target;

    private void Start()
    {
        MoveWall();
    }

    public void MoveWall()
    {
        transform.position = target.position;
    }
}
