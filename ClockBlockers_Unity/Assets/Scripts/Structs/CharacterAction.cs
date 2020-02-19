using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAction
{
    public String method;
    public String parameter;
    public float time;

    public override string ToString()
    {
        return method + "should be called " + time + " seconds after spawn.";
    }
}
