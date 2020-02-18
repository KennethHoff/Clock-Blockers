using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAction
{
    public String action;
    public float time;

    public override string ToString()
    {
        return action + "should be called " + time + " seconds after spawn.";
    }
}
