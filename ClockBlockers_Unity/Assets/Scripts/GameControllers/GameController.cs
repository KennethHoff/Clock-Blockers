using System;
using System.Collections;
using System.Collections.Generic;
using DataStructures;
using UnityEngine;
using Utility;

public class GameController : MonoBehaviour
{


    public static GameController instance;
    public Transform cloneParent;
    public Material deadMaterial;

    public GameObject[] bulletHoles;

    [Range(1, 6)]public int floatingPointPrecision = 6;

    public String FloatPointPrecisionString { get => "F" + floatingPointPrecision; }

    void Awake()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        instance = this;
    }
}
