using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController gc;
    public Transform cloneParent;
    public Material deadMaterial;

    void Awake()
    {
        gc = this;
    }

}
