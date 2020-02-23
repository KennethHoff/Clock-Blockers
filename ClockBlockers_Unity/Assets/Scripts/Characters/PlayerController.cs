using System;
using System.Collections;
using System.Collections.Generic;
using ClockBlockers.DataStructures;
using ClockBlockers.GameControllers;
using ClockBlockers.Utility;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ClockBlockers.Characters
{
    public class PlayerController : BaseController
    {
        [Header("Setup Variables")]

        //private List<String> currentFrameActions;
        private List<Tuple<Actions, string>> _currentFrameActions;

        private Vector2 _movementInput;

        // ReSharper disable once IdentifierTypo
        private float _upDowncameraRotation;
        private float _sideToSideCharacterRotation;

        [Header("Player Setting")]
        public float verticalMouseSensitivity;

        public float horizontalMouseSensitivity;

        protected override void Awake()
        {
            base.Awake();
            _currentFrameActions = new List<Tuple<Actions, string>>();
            characterActions = new List<CharacterAction>();
        }

        protected override void FixedUpdate()
        {
            MoveCharacterByInput();

            SaveCharacterActions();

            base.FixedUpdate();
        }

        private void Update()
        {
            RotateCharacter(_sideToSideCharacterRotation);
            RotateCamera(_upDowncameraRotation);
        }

        [UsedImplicitly]
        private void OnLook(InputValue ctx)
        {
            var value = ctx.Get<Vector2>();

            _sideToSideCharacterRotation = value.x * horizontalMouseSensitivity * Time.deltaTime;
            _upDowncameraRotation = value.y * verticalMouseSensitivity * Time.deltaTime;
        }

        [UsedImplicitly]
        private void OnMovement(InputValue ctx)
        {
            _movementInput = ctx.Get<Vector2>();
        }

        [UsedImplicitly]
        private void OnSpawn()
        {
            StartCoroutine(WaitSpawnClone());
        }

        [UsedImplicitly]
        private void OnJump()
        {
            StartCoroutine(WaitAttemptToJump());
        }

        [UsedImplicitly]
        private void OnShoot(InputValue ctx)
        {
            if (ctx.isPressed) StartCoroutine(WaitAttemptToShoot());
        }

        [UsedImplicitly]
        private void OnClearClones()
        {
            Logging.Log("Clearing children");
            for (var i = 0;
                i < GameController.instance.cloneParent.childCount;
                i++)
            {
                Destroy(GameController.instance.cloneParent.GetChild(i).gameObject);
            }
        }

        [UsedImplicitly]
        private void OnIncreaseTimescale()
        {
            Time.timeScale += 1;
            Logging.Log("Increasing timescale. Now at: " + Time.timeScale);
        }

        [UsedImplicitly]
        private void OnDecreaseTimescale()
        {
            Time.timeScale -= 1;
            Logging.Log("Decreasing timescale. Now at: " + Time.timeScale);
        }

        private void SaveCharacterActions()
        {
            if (!recordActions) return;
            foreach ((Actions actions, string parameter) in _currentFrameActions)
            {
                var newAction = new CharacterAction
                {
                    action = actions, parameter = parameter, time = Time.fixedTime - spawnTime,
                };
                characterActions.Add(newAction);
            }

            _currentFrameActions.Clear();
        }

        private void SaveActionAsString(Actions action, string parameters)
        {
            _currentFrameActions.Add(Tuple.Create(action, parameters));

            if (debugLogEveryAction)
                // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            {
                Logging.Log("Time: " + Time.time + ". Function: " + action + ". Parameters:" + parameters, this);
            }
        }

        private void SaveActionAsString(Actions action)
        {
            SaveActionAsString(action, "");
        }

        protected override void RotateCharacter(float rotation)
        {
            var stringedFloat = rotation.ToString(GameController.instance.FloatPointPrecisionString);
            var roundedFloat = float.Parse(stringedFloat);

            SaveActionAsString(Actions.RotateCharacter, stringedFloat);
            base.RotateCharacter(roundedFloat);
        }

        protected override void RotateCamera(float rotation)
        {
            var stringedFloat = rotation.ToString(GameController.instance.FloatPointPrecisionString);
            float roundedFloat = float.Parse(stringedFloat);

            SaveActionAsString(Actions.RotateCamera, stringedFloat);
            base.RotateCamera(roundedFloat);
        }

        private void MoveCharacterByInput()
        {
            // If no input, magnitude = 0. I don't want it to record every frame for all eternity. Only when moving.
            if (_movementInput.magnitude < 0.1f) return; 

            Vector2 timeAdjustedInput = _movementInput * Time.fixedDeltaTime;
            MoveCharacterForward(new Vector3(timeAdjustedInput.x, 0, timeAdjustedInput.y));
        }

        protected override void MoveCharacterForward(Vector3 vector)
        {
            Vector3 roundedVector = vector.Round(GameController.instance.floatingPointPrecision);
            var stringedVector = roundedVector.ToString(GameController.instance.FloatPointPrecisionString);

            SaveActionAsString(Actions.Move, stringedVector);
            base.MoveCharacterForward(roundedVector);
        }

        protected override void AttemptToJump()
        {
            SaveActionAsString(Actions.Jump);
            base.AttemptToJump();
        }

        protected override void AttemptToShoot()
        {
            SaveActionAsString(Actions.Shoot);
            base.AttemptToShoot();
        }

        protected override void SpawnClone()
        {
            if (enableRecursiveClones) SaveActionAsString(Actions.SpawnClone);

            // HACK: BAD PRACTICE ALERT!
            SaveCharacterActions();

            //for (int i = 0; i < 100; i++)
            //{
            base.SpawnClone();
            //}
        }

        private IEnumerator WaitAttemptToJump()
        {
            yield return new WaitForFixedUpdate();
            AttemptToJump();
        }

        private IEnumerator WaitAttemptToShoot()
        {
            yield return new WaitForFixedUpdate();
            AttemptToShoot();
        }

        private IEnumerator WaitSpawnClone()
        {
            yield return new WaitForFixedUpdate();
            SpawnClone();
        }
    }
}