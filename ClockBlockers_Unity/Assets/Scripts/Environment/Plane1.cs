﻿using ClockBlockers.DataStructures;
using UnityEngine;

namespace ClockBlockers.Environment {
    public class Plane1 : MonoBehaviour, IInteractable
    {
        public void OnHit(DamagePacket damagePacket, Vector3 hitPosition)
        {
            //Logging.Log("Shot a Plane!");
        }
    }
}