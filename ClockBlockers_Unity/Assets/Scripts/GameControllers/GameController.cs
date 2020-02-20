using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class GameController : MonoBehaviour
{
    public static GameController gc;
    public Transform cloneParent;
    public Material deadMaterial;

    public GameObject[] bulletHoles;

    public LayerMask terrainLayer;
    [HideInInspector] public int terrainLayerID;

    public LayerMask targetLayer;
    [HideInInspector] public int targetLayerID;

    [Range(1, 6)]public int floatingPointPrecision = 6;

    public String FloatPointPrecisionString { get => "F" + floatingPointPrecision; }

    void Awake()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        gc = this;
        terrainLayerID = UsefulMethods.ToLayer(terrainLayer);
        targetLayerID = UsefulMethods.ToLayer(targetLayer);
    }

    void Update()
    {
        if (Time.unscaledDeltaTime > 1f)
        {
            Debug.LogWarning("Game too slow. Stopped it");
            Debug.Break();
        }
    }

}
