using System;

namespace DataStructures
{
    public class CharacterAction
    {
        public Actions action;
        public String parameter;
        public float time;
        
        public override string ToString()
        {
            return action + "should be called " + time + " seconds after spawn.";
        }
    }

    public enum Actions
    {
        Move,
        RotateCharacter, // Read under
        RotateCamera, // Should probably be merged into a single one.
        Jump,
        Shoot,
        SpawnClone
    }
}