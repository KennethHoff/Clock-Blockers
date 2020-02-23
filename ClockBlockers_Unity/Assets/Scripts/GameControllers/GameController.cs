using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ClockBlockers.GameControllers {
    public class GameController : MonoBehaviour
    {

        public string firstScene;

        public static GameController instance;
        public Transform cloneParent;
        public Material deadMaterial;

        public GameObject[] bulletHoles;

        [Range(1, 6)]public int floatingPointPrecision = 6;

        public String FloatPointPrecisionString { get => "F" + floatingPointPrecision; }

        void Awake()
        {
            SceneManager.LoadScene(firstScene, LoadSceneMode.Additive);
            //Cursor.lockState = CursorLockMode.Locked;
            instance = this;
        }
    }
}
