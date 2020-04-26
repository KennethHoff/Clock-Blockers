// GENERATED AUTOMATICALLY FROM 'Assets/PlayerActions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;


namespace ClockBlockers.Input
{
	public class @PlayerActions : IInputActionCollection, IDisposable
	{
		public InputActionAsset asset { get; }

		public @PlayerActions()
		{
			asset = InputActionAsset.FromJson(@"{
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
                },
                {
                    ""name"": ""SaveCharacterActions"",
                    ""type"": ""Button"",
                    ""id"": ""46eb3060-8583-4cdd-990a-362c1e156ab4"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""LoadCharacterActions"",
                    ""type"": ""Button"",
                    ""id"": ""49e2be10-190a-4f7d-a9b8-0c0ad602ff32"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ResetRound"",
                    ""type"": ""Button"",
                    ""id"": ""ba7f09f9-5c35-47cd-97f3-ac759692732b"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ToggleCursor"",
                    ""type"": ""Button"",
                    ""id"": ""999cb916-1a4c-4f0d-bf6e-7b02916a080b"",
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
                },
                {
                    ""name"": """",
                    ""id"": ""4dfabc1f-0d0a-4296-a799-a5c0dee1d38c"",
                    ""path"": ""<Keyboard>/f5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KB&M"",
                    ""action"": ""SaveCharacterActions"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""906f3ab5-eff8-4715-9f8e-78baaaefae52"",
                    ""path"": ""<Keyboard>/f9"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KB&M"",
                    ""action"": ""LoadCharacterActions"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""984d5f39-a92c-41ac-92e1-8451a3e7e081"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KB&M"",
                    ""action"": ""ResetRound"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0bcff2a9-1e34-41f5-934a-db35c9ca3607"",
                    ""path"": ""<Keyboard>/leftAlt"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KB&M"",
                    ""action"": ""ToggleCursor"",
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
			m_Character = asset.FindActionMap("Character", throwIfNotFound: true);
			m_Character_Movement = m_Character.FindAction("Movement", throwIfNotFound: true);
			m_Character_Look = m_Character.FindAction("Look", throwIfNotFound: true);
			m_Character_Shoot = m_Character.FindAction("Shoot", throwIfNotFound: true);
			m_Character_Jump = m_Character.FindAction("Jump", throwIfNotFound: true);
			m_Character_Spawn = m_Character.FindAction("Spawn", throwIfNotFound: true);
			m_Character_ClearClones = m_Character.FindAction("ClearClones", throwIfNotFound: true);
			m_Character_IncreaseTimescale = m_Character.FindAction("IncreaseTimescale", throwIfNotFound: true);
			m_Character_DecreaseTimescale = m_Character.FindAction("DecreaseTimescale", throwIfNotFound: true);
			m_Character_Pause = m_Character.FindAction("Pause", throwIfNotFound: true);
			m_Character_SaveCharacterActions = m_Character.FindAction("SaveCharacterActions", throwIfNotFound: true);
			m_Character_LoadCharacterActions = m_Character.FindAction("LoadCharacterActions", throwIfNotFound: true);
			m_Character_ResetRound = m_Character.FindAction("ResetRound", throwIfNotFound: true);
			m_Character_ToggleCursor = m_Character.FindAction("ToggleCursor", throwIfNotFound: true);
		}

		public void Dispose()
		{
			UnityEngine.Object.Destroy(asset);
		}

		public InputBinding? bindingMask
		{
			get => asset.bindingMask;
			set => asset.bindingMask = value;
		}

		public ReadOnlyArray<InputDevice>? devices
		{
			get => asset.devices;
			set => asset.devices = value;
		}

		public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

		public bool Contains(InputAction action)
		{
			return asset.Contains(action);
		}

		public IEnumerator<InputAction> GetEnumerator()
		{
			return asset.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Enable()
		{
			asset.Enable();
		}

		public void Disable()
		{
			asset.Disable();
		}

		// Character
		private readonly InputActionMap m_Character;
		private ICharacterActions m_CharacterActionsCallbackInterface;
		private readonly InputAction m_Character_Movement;
		private readonly InputAction m_Character_Look;
		private readonly InputAction m_Character_Shoot;
		private readonly InputAction m_Character_Jump;
		private readonly InputAction m_Character_Spawn;
		private readonly InputAction m_Character_ClearClones;
		private readonly InputAction m_Character_IncreaseTimescale;
		private readonly InputAction m_Character_DecreaseTimescale;
		private readonly InputAction m_Character_Pause;
		private readonly InputAction m_Character_SaveCharacterActions;
		private readonly InputAction m_Character_LoadCharacterActions;
		private readonly InputAction m_Character_ResetRound;
		private readonly InputAction m_Character_ToggleCursor;

		public struct CharacterActions
		{
			private @PlayerActions m_Wrapper;

			public CharacterActions(@PlayerActions wrapper)
			{
				m_Wrapper = wrapper;
			}

			public InputAction @Movement => m_Wrapper.m_Character_Movement;
			public InputAction @Look => m_Wrapper.m_Character_Look;
			public InputAction @Shoot => m_Wrapper.m_Character_Shoot;
			public InputAction @Jump => m_Wrapper.m_Character_Jump;
			public InputAction @Spawn => m_Wrapper.m_Character_Spawn;
			public InputAction @ClearClones => m_Wrapper.m_Character_ClearClones;
			public InputAction @IncreaseTimescale => m_Wrapper.m_Character_IncreaseTimescale;
			public InputAction @DecreaseTimescale => m_Wrapper.m_Character_DecreaseTimescale;
			public InputAction @Pause => m_Wrapper.m_Character_Pause;
			public InputAction @SaveCharacterActions => m_Wrapper.m_Character_SaveCharacterActions;
			public InputAction @LoadCharacterActions => m_Wrapper.m_Character_LoadCharacterActions;
			public InputAction @ResetRound => m_Wrapper.m_Character_ResetRound;
			public InputAction @ToggleCursor => m_Wrapper.m_Character_ToggleCursor;

			public InputActionMap Get()
			{
				return m_Wrapper.m_Character;
			}

			public void Enable()
			{
				Get().Enable();
			}

			public void Disable()
			{
				Get().Disable();
			}

			public bool enabled => Get().enabled;

			public static implicit operator InputActionMap(CharacterActions set)
			{
				return set.Get();
			}

			public void SetCallbacks(ICharacterActions instance)
			{
				if (m_Wrapper.m_CharacterActionsCallbackInterface != null)
				{
					@Movement.started -= m_Wrapper.m_CharacterActionsCallbackInterface.OnMovement;
					@Movement.performed -= m_Wrapper.m_CharacterActionsCallbackInterface.OnMovement;
					@Movement.canceled -= m_Wrapper.m_CharacterActionsCallbackInterface.OnMovement;
					@Look.started -= m_Wrapper.m_CharacterActionsCallbackInterface.OnLook;
					@Look.performed -= m_Wrapper.m_CharacterActionsCallbackInterface.OnLook;
					@Look.canceled -= m_Wrapper.m_CharacterActionsCallbackInterface.OnLook;
					@Shoot.started -= m_Wrapper.m_CharacterActionsCallbackInterface.OnShoot;
					@Shoot.performed -= m_Wrapper.m_CharacterActionsCallbackInterface.OnShoot;
					@Shoot.canceled -= m_Wrapper.m_CharacterActionsCallbackInterface.OnShoot;
					@Jump.started -= m_Wrapper.m_CharacterActionsCallbackInterface.OnJump;
					@Jump.performed -= m_Wrapper.m_CharacterActionsCallbackInterface.OnJump;
					@Jump.canceled -= m_Wrapper.m_CharacterActionsCallbackInterface.OnJump;
					@Spawn.started -= m_Wrapper.m_CharacterActionsCallbackInterface.OnSpawn;
					@Spawn.performed -= m_Wrapper.m_CharacterActionsCallbackInterface.OnSpawn;
					@Spawn.canceled -= m_Wrapper.m_CharacterActionsCallbackInterface.OnSpawn;
					@ClearClones.started -= m_Wrapper.m_CharacterActionsCallbackInterface.OnClearClones;
					@ClearClones.performed -= m_Wrapper.m_CharacterActionsCallbackInterface.OnClearClones;
					@ClearClones.canceled -= m_Wrapper.m_CharacterActionsCallbackInterface.OnClearClones;
					@IncreaseTimescale.started -= m_Wrapper.m_CharacterActionsCallbackInterface.OnIncreaseTimescale;
					@IncreaseTimescale.performed -= m_Wrapper.m_CharacterActionsCallbackInterface.OnIncreaseTimescale;
					@IncreaseTimescale.canceled -= m_Wrapper.m_CharacterActionsCallbackInterface.OnIncreaseTimescale;
					@DecreaseTimescale.started -= m_Wrapper.m_CharacterActionsCallbackInterface.OnDecreaseTimescale;
					@DecreaseTimescale.performed -= m_Wrapper.m_CharacterActionsCallbackInterface.OnDecreaseTimescale;
					@DecreaseTimescale.canceled -= m_Wrapper.m_CharacterActionsCallbackInterface.OnDecreaseTimescale;
					@Pause.started -= m_Wrapper.m_CharacterActionsCallbackInterface.OnPause;
					@Pause.performed -= m_Wrapper.m_CharacterActionsCallbackInterface.OnPause;
					@Pause.canceled -= m_Wrapper.m_CharacterActionsCallbackInterface.OnPause;
					@SaveCharacterActions.started -= m_Wrapper.m_CharacterActionsCallbackInterface.OnSaveCharacterActions;
					@SaveCharacterActions.performed -= m_Wrapper.m_CharacterActionsCallbackInterface.OnSaveCharacterActions;
					@SaveCharacterActions.canceled -= m_Wrapper.m_CharacterActionsCallbackInterface.OnSaveCharacterActions;
					@LoadCharacterActions.started -= m_Wrapper.m_CharacterActionsCallbackInterface.OnLoadCharacterActions;
					@LoadCharacterActions.performed -= m_Wrapper.m_CharacterActionsCallbackInterface.OnLoadCharacterActions;
					@LoadCharacterActions.canceled -= m_Wrapper.m_CharacterActionsCallbackInterface.OnLoadCharacterActions;
					@ResetRound.started -= m_Wrapper.m_CharacterActionsCallbackInterface.OnResetRound;
					@ResetRound.performed -= m_Wrapper.m_CharacterActionsCallbackInterface.OnResetRound;
					@ResetRound.canceled -= m_Wrapper.m_CharacterActionsCallbackInterface.OnResetRound;
					@ToggleCursor.started -= m_Wrapper.m_CharacterActionsCallbackInterface.OnToggleCursor;
					@ToggleCursor.performed -= m_Wrapper.m_CharacterActionsCallbackInterface.OnToggleCursor;
					@ToggleCursor.canceled -= m_Wrapper.m_CharacterActionsCallbackInterface.OnToggleCursor;
				}

				m_Wrapper.m_CharacterActionsCallbackInterface = instance;
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
					@SaveCharacterActions.started += instance.OnSaveCharacterActions;
					@SaveCharacterActions.performed += instance.OnSaveCharacterActions;
					@SaveCharacterActions.canceled += instance.OnSaveCharacterActions;
					@LoadCharacterActions.started += instance.OnLoadCharacterActions;
					@LoadCharacterActions.performed += instance.OnLoadCharacterActions;
					@LoadCharacterActions.canceled += instance.OnLoadCharacterActions;
					@ResetRound.started += instance.OnResetRound;
					@ResetRound.performed += instance.OnResetRound;
					@ResetRound.canceled += instance.OnResetRound;
					@ToggleCursor.started += instance.OnToggleCursor;
					@ToggleCursor.performed += instance.OnToggleCursor;
					@ToggleCursor.canceled += instance.OnToggleCursor;
				}
			}
		}

		public CharacterActions @Character => new CharacterActions(this);
		private int m_KBMSchemeIndex = -1;

		public InputControlScheme KBMScheme
		{
			get
			{
				if (m_KBMSchemeIndex == -1) m_KBMSchemeIndex = asset.FindControlSchemeIndex("KB&M");
				return asset.controlSchemes[m_KBMSchemeIndex];
			}
		}

		private int m_GamepadSchemeIndex = -1;

		public InputControlScheme GamepadScheme
		{
			get
			{
				if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
				return asset.controlSchemes[m_GamepadSchemeIndex];
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
			void OnSaveCharacterActions(InputAction.CallbackContext context);
			void OnLoadCharacterActions(InputAction.CallbackContext context);
			void OnResetRound(InputAction.CallbackContext context);
			void OnToggleCursor(InputAction.CallbackContext context);
		}
	}
}