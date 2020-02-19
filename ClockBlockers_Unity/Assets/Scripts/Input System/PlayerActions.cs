// GENERATED AUTOMATICALLY FROM 'Assets/PlayerActions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

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
                    ""name"": ""ShootRayCast"",
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
                    ""method"": ""Movement"",
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
                    ""method"": ""Movement"",
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
                    ""method"": ""Movement"",
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
                    ""method"": ""Movement"",
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
                    ""method"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""b325f041-5c75-4bbe-8b60-140f72d8686e"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KB&M"",
                    ""method"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Arrows"",
                    ""id"": ""68cc3664-bae7-4243-8e04-baf2e58f8148"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": ""ScaleVector2(x=25,y=25)"",
                    ""groups"": """",
                    ""method"": ""Look"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""c733775e-5175-48cb-97b0-9344a84c19a8"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KB&M"",
                    ""method"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""c5f3715f-7708-4a72-898a-379c9b91c9cf"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KB&M"",
                    ""method"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""672be28c-40aa-4daf-a8bf-3aa2e3e09243"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KB&M"",
                    ""method"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""b975e7aa-e53b-44df-b3b2-e66996ce8022"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KB&M"",
                    ""method"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""ddb6c169-c21a-44d5-90e8-44eb6ae2e1a2"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KB&M"",
                    ""method"": ""Spawn"",
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
                    ""method"": ""ClearClones"",
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
                    ""method"": ""IncreaseTimescale"",
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
                    ""method"": ""DecreaseTimescale"",
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
                    ""method"": ""Jump"",
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
                    ""method"": ""ShootRayCast"",
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
        }
    ]
}");
        // Character
        m_Character = asset.FindActionMap("Character", throwIfNotFound: true);
        m_Character_Movement = m_Character.FindAction("Movement", throwIfNotFound: true);
        m_Character_Look = m_Character.FindAction("Look", throwIfNotFound: true);
        m_Character_Shoot = m_Character.FindAction("ShootRayCast", throwIfNotFound: true);
        m_Character_Jump = m_Character.FindAction("Jump", throwIfNotFound: true);
        m_Character_Spawn = m_Character.FindAction("Spawn", throwIfNotFound: true);
        m_Character_ClearClones = m_Character.FindAction("ClearClones", throwIfNotFound: true);
        m_Character_IncreaseTimescale = m_Character.FindAction("IncreaseTimescale", throwIfNotFound: true);
        m_Character_DecreaseTimescale = m_Character.FindAction("DecreaseTimescale", throwIfNotFound: true);
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
    public struct CharacterActions
    {
        private @PlayerActions m_Wrapper;
        public CharacterActions(@PlayerActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_Character_Movement;
        public InputAction @Look => m_Wrapper.m_Character_Look;
        public InputAction @Shoot => m_Wrapper.m_Character_Shoot;
        public InputAction @Jump => m_Wrapper.m_Character_Jump;
        public InputAction @Spawn => m_Wrapper.m_Character_Spawn;
        public InputAction @ClearClones => m_Wrapper.m_Character_ClearClones;
        public InputAction @IncreaseTimescale => m_Wrapper.m_Character_IncreaseTimescale;
        public InputAction @DecreaseTimescale => m_Wrapper.m_Character_DecreaseTimescale;
        public InputActionMap Get() { return m_Wrapper.m_Character; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CharacterActions set) { return set.Get(); }
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
    }
}
