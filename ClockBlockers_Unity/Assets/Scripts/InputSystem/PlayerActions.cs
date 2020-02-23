// GENERATED AUTOMATICALLY FROM 'Assets/PlayerActions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace ClockBlockers.InputSystem {
    public class @PlayerActions : IInputActionCollection, IDisposable
    {
        public InputActionAsset Asset { get; }
        public @PlayerActions()
        {
            Asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerActions"",
    ""maps"": [
        {
            ""name"": ""Character"",
            ""id"": ""2af3e19f-1896-4574-82ec-de14edb382f6"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""d1df5305-4622-435e-8468-30e86c3d92c5"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""e011e656-f6e8-4469-bfa9-e4031f599e34"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Shoot"",
                    ""type"": ""Button"",
                    ""id"": ""0cd467c4-6c6b-42ef-8dd7-efc7e57e4401"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""60daafb2-1bbd-4a40-a470-59e12d23fae4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Spawn"",
                    ""type"": ""Button"",
                    ""id"": ""25542b2e-fc3a-4122-8b48-f26141e42ee2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ClearClones"",
                    ""type"": ""Button"",
                    ""id"": ""8210257b-56ab-44a3-8638-ce5a796c27c7"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""IncreaseTimescale"",
                    ""type"": ""Button"",
                    ""id"": ""589b545d-6d4b-445b-be0f-2345ef733a81"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""DecreaseTimescale"",
                    ""type"": ""Button"",
                    ""id"": ""335657d1-cbf6-46c2-9f68-9ae8cc7bf218"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""39a1958e-0deb-4588-82aa-f695472356f5"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""17ed85c2-9abf-4e77-bf9f-137d9d9f4823"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""8b4348d9-be8b-4fc9-825a-2d02b35ff495"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KB&M"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""b2a2c995-894b-4a19-9703-a88295021ccd"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KB&M"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""2b839598-af87-456e-b4e4-c11a3b1e1ee2"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KB&M"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""b67bd04e-2d00-42e6-ad93-1a72501a5cfc"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KB&M"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""8f057c0d-cc12-45f7-a4ee-a2dfa4983a11"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b325f041-5c75-4bbe-8b60-140f72d8686e"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KB&M"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e0fbfcfe-3da9-4d7a-a724-91cd2f0b59ac"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": ""ScaleVector2(x=20,y=20)"",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ddb6c169-c21a-44d5-90e8-44eb6ae2e1a2"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KB&M"",
                    ""action"": ""Spawn"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5c891853-e4a6-4cc3-8d99-29e3ff8bc463"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Spawn"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9de355b4-5824-4464-9687-6a2128bdccc2"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KB&M"",
                    ""action"": ""ClearClones"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4eb4a551-b433-4fd9-9101-1c6d01bc22eb"",
                    ""path"": ""<Gamepad>/select"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""ClearClones"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""649e3dad-e6f7-4170-801a-6f7c002cd34c"",
                    ""path"": ""<Keyboard>/numpadPlus"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KB&M"",
                    ""action"": ""IncreaseTimescale"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c4b27fd9-3463-4c72-8813-f2d48462b17f"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""IncreaseTimescale"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""35ef55b7-5baf-4a61-94b9-3519aad13adc"",
                    ""path"": ""<Keyboard>/numpadMinus"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KB&M"",
                    ""action"": ""DecreaseTimescale"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5ef99235-cfd1-4c94-8624-b40d7af51d23"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""DecreaseTimescale"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d23e72fb-7057-4d96-9606-50b0366a008c"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KB&M"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d7c24545-7299-47de-a4e1-8d52023cb851"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7a1cc27f-7660-4874-8052-a40007957a06"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": ""KB&M"",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fc981997-f26e-4cf0-b366-a6518291be95"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""80ed81bf-3cd0-44d3-b833-22e6d5fced94"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KB&M"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""45f71433-fff0-4671-9d65-079ba69cb448"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""KB&M"",
            ""bindingGroup"": ""KB&M"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
            // Character
            _mCharacter = Asset.FindActionMap("Character", throwIfNotFound: true);
            _mCharacterMovement = _mCharacter.FindAction("Movement", throwIfNotFound: true);
            _mCharacterLook = _mCharacter.FindAction("Look", throwIfNotFound: true);
            _mCharacterShoot = _mCharacter.FindAction("Shoot", throwIfNotFound: true);
            _mCharacterJump = _mCharacter.FindAction("Jump", throwIfNotFound: true);
            _mCharacterSpawn = _mCharacter.FindAction("Spawn", throwIfNotFound: true);
            _mCharacterClearClones = _mCharacter.FindAction("ClearClones", throwIfNotFound: true);
            _mCharacterIncreaseTimescale = _mCharacter.FindAction("IncreaseTimescale", throwIfNotFound: true);
            _mCharacterDecreaseTimescale = _mCharacter.FindAction("DecreaseTimescale", throwIfNotFound: true);
            _mCharacterPause = _mCharacter.FindAction("Pause", throwIfNotFound: true);
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(Asset);
        }

        public InputBinding? bindingMask
        {
            get => Asset.bindingMask;
            set => Asset.bindingMask = value;
        }

        public ReadOnlyArray<InputDevice>? devices
        {
            get => Asset.devices;
            set => Asset.devices = value;
        }

        public ReadOnlyArray<InputControlScheme> controlSchemes => Asset.controlSchemes;

        public bool Contains(InputAction action)
        {
            return Asset.Contains(action);
        }

        public IEnumerator<InputAction> GetEnumerator()
        {
            return Asset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Enable()
        {
            Asset.Enable();
        }

        public void Disable()
        {
            Asset.Disable();
        }

        // Character
        private readonly InputActionMap _mCharacter;
        private ICharacterActions _mCharacterActionsCallbackInterface;
        private readonly InputAction _mCharacterMovement;
        private readonly InputAction _mCharacterLook;
        private readonly InputAction _mCharacterShoot;
        private readonly InputAction _mCharacterJump;
        private readonly InputAction _mCharacterSpawn;
        private readonly InputAction _mCharacterClearClones;
        private readonly InputAction _mCharacterIncreaseTimescale;
        private readonly InputAction _mCharacterDecreaseTimescale;
        private readonly InputAction _mCharacterPause;
        public struct CharacterActions
        {
            private @PlayerActions _mWrapper;
            public CharacterActions(@PlayerActions wrapper) { _mWrapper = wrapper; }
            public InputAction @Movement => _mWrapper._mCharacterMovement;
            public InputAction @Look => _mWrapper._mCharacterLook;
            public InputAction @Shoot => _mWrapper._mCharacterShoot;
            public InputAction @Jump => _mWrapper._mCharacterJump;
            public InputAction @Spawn => _mWrapper._mCharacterSpawn;
            public InputAction @ClearClones => _mWrapper._mCharacterClearClones;
            public InputAction @IncreaseTimescale => _mWrapper._mCharacterIncreaseTimescale;
            public InputAction @DecreaseTimescale => _mWrapper._mCharacterDecreaseTimescale;
            public InputAction @Pause => _mWrapper._mCharacterPause;
            public InputActionMap Get() { return _mWrapper._mCharacter; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool Enabled => Get().enabled;
            public static implicit operator InputActionMap(CharacterActions set) { return set.Get(); }
            public void SetCallbacks(ICharacterActions instance)
            {
                if (_mWrapper._mCharacterActionsCallbackInterface != null)
                {
                    @Movement.started -= _mWrapper._mCharacterActionsCallbackInterface.OnMovement;
                    @Movement.performed -= _mWrapper._mCharacterActionsCallbackInterface.OnMovement;
                    @Movement.canceled -= _mWrapper._mCharacterActionsCallbackInterface.OnMovement;
                    @Look.started -= _mWrapper._mCharacterActionsCallbackInterface.OnLook;
                    @Look.performed -= _mWrapper._mCharacterActionsCallbackInterface.OnLook;
                    @Look.canceled -= _mWrapper._mCharacterActionsCallbackInterface.OnLook;
                    @Shoot.started -= _mWrapper._mCharacterActionsCallbackInterface.OnShoot;
                    @Shoot.performed -= _mWrapper._mCharacterActionsCallbackInterface.OnShoot;
                    @Shoot.canceled -= _mWrapper._mCharacterActionsCallbackInterface.OnShoot;
                    @Jump.started -= _mWrapper._mCharacterActionsCallbackInterface.OnJump;
                    @Jump.performed -= _mWrapper._mCharacterActionsCallbackInterface.OnJump;
                    @Jump.canceled -= _mWrapper._mCharacterActionsCallbackInterface.OnJump;
                    @Spawn.started -= _mWrapper._mCharacterActionsCallbackInterface.OnSpawn;
                    @Spawn.performed -= _mWrapper._mCharacterActionsCallbackInterface.OnSpawn;
                    @Spawn.canceled -= _mWrapper._mCharacterActionsCallbackInterface.OnSpawn;
                    @ClearClones.started -= _mWrapper._mCharacterActionsCallbackInterface.OnClearClones;
                    @ClearClones.performed -= _mWrapper._mCharacterActionsCallbackInterface.OnClearClones;
                    @ClearClones.canceled -= _mWrapper._mCharacterActionsCallbackInterface.OnClearClones;
                    @IncreaseTimescale.started -= _mWrapper._mCharacterActionsCallbackInterface.OnIncreaseTimescale;
                    @IncreaseTimescale.performed -= _mWrapper._mCharacterActionsCallbackInterface.OnIncreaseTimescale;
                    @IncreaseTimescale.canceled -= _mWrapper._mCharacterActionsCallbackInterface.OnIncreaseTimescale;
                    @DecreaseTimescale.started -= _mWrapper._mCharacterActionsCallbackInterface.OnDecreaseTimescale;
                    @DecreaseTimescale.performed -= _mWrapper._mCharacterActionsCallbackInterface.OnDecreaseTimescale;
                    @DecreaseTimescale.canceled -= _mWrapper._mCharacterActionsCallbackInterface.OnDecreaseTimescale;
                    @Pause.started -= _mWrapper._mCharacterActionsCallbackInterface.OnPause;
                    @Pause.performed -= _mWrapper._mCharacterActionsCallbackInterface.OnPause;
                    @Pause.canceled -= _mWrapper._mCharacterActionsCallbackInterface.OnPause;
                }
                _mWrapper._mCharacterActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Movement.started += instance.OnMovement;
                    @Movement.performed += instance.OnMovement;
                    @Movement.canceled += instance.OnMovement;
                    @Look.started += instance.OnLook;
                    @Look.performed += instance.OnLook;
                    @Look.canceled += instance.OnLook;
                    @Shoot.started += instance.OnShoot;
                    @Shoot.performed += instance.OnShoot;
                    @Shoot.canceled += instance.OnShoot;
                    @Jump.started += instance.OnJump;
                    @Jump.performed += instance.OnJump;
                    @Jump.canceled += instance.OnJump;
                    @Spawn.started += instance.OnSpawn;
                    @Spawn.performed += instance.OnSpawn;
                    @Spawn.canceled += instance.OnSpawn;
                    @ClearClones.started += instance.OnClearClones;
                    @ClearClones.performed += instance.OnClearClones;
                    @ClearClones.canceled += instance.OnClearClones;
                    @IncreaseTimescale.started += instance.OnIncreaseTimescale;
                    @IncreaseTimescale.performed += instance.OnIncreaseTimescale;
                    @IncreaseTimescale.canceled += instance.OnIncreaseTimescale;
                    @DecreaseTimescale.started += instance.OnDecreaseTimescale;
                    @DecreaseTimescale.performed += instance.OnDecreaseTimescale;
                    @DecreaseTimescale.canceled += instance.OnDecreaseTimescale;
                    @Pause.started += instance.OnPause;
                    @Pause.performed += instance.OnPause;
                    @Pause.canceled += instance.OnPause;
                }
            }
        }
        public CharacterActions @Character => new CharacterActions(this);
        private int _mKbmSchemeIndex = -1;
        public InputControlScheme KbmScheme
        {
            get
            {
                if (_mKbmSchemeIndex == -1) _mKbmSchemeIndex = Asset.FindControlSchemeIndex("KB&M");
                return Asset.controlSchemes[_mKbmSchemeIndex];
            }
        }
        private int _mGamepadSchemeIndex = -1;
        public InputControlScheme GamepadScheme
        {
            get
            {
                if (_mGamepadSchemeIndex == -1) _mGamepadSchemeIndex = Asset.FindControlSchemeIndex("Gamepad");
                return Asset.controlSchemes[_mGamepadSchemeIndex];
            }
        }
        public interface ICharacterActions
        {
            void OnMovement(InputAction.CallbackContext context);
            void OnLook(InputAction.CallbackContext context);
            void OnShoot(InputAction.CallbackContext context);
            void OnJump(InputAction.CallbackContext context);
            void OnSpawn(InputAction.CallbackContext context);
            void OnClearClones(InputAction.CallbackContext context);
            void OnIncreaseTimescale(InputAction.CallbackContext context);
            void OnDecreaseTimescale(InputAction.CallbackContext context);
            void OnPause(InputAction.CallbackContext context);
        }
    }
}
