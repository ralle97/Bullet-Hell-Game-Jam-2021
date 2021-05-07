// GENERATED AUTOMATICALLY FROM 'Assets/Controls/Controls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Controls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Controls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controls"",
    ""maps"": [
        {
            ""name"": ""Master"",
            ""id"": ""c71489d7-9820-4d0b-b2d8-06ab91acbd12"",
            ""actions"": [
                {
                    ""name"": ""UpgradeMenu"",
                    ""type"": ""Button"",
                    ""id"": ""2c8729b5-4bf1-456d-96ba-db788a21f8f7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""e924fad8-4c9b-4250-b658-8cbbc7548c93"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Back"",
                    ""type"": ""Button"",
                    ""id"": ""7fd4debc-ed9e-4329-b4fd-ed3030eeacfb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MousePosition"",
                    ""type"": ""Value"",
                    ""id"": ""8e5b5844-2b53-4af6-9832-9c6e3c3edbe8"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""cab8b9be-eb2b-4c3f-9fd7-056d97ff521b"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Aim"",
                    ""type"": ""Value"",
                    ""id"": ""b8dd1346-9ac7-40e8-8e87-25264a4e4b74"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Shoot"",
                    ""type"": ""Button"",
                    ""id"": ""c1caaac2-8c70-488d-85bc-03cbac1953b6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""8b51e02a-a0de-4d31-b889-585de623fc43"",
                    ""path"": ""<Keyboard>/u"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UpgradeMenu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""066ea1d4-be02-4cf2-b976-933d526fcecb"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UpgradeMenu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9bb49e9c-1046-4977-b0e5-48a40b1e969b"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7ce977f3-bb44-4e8a-a938-e525ae725a0d"",
                    ""path"": ""<Keyboard>/p"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e2d18996-415d-4499-9522-39026f5fae79"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""389f23a3-a477-46d5-9347-10993cbb5ea6"",
                    ""path"": ""<Keyboard>/backspace"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Back"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""92f7b4c7-ed46-42f1-9a14-76caefd5debf"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Back"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8536bd92-104b-420e-b74f-64da9a937577"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MousePosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""89b88777-ac4f-407c-8caa-294c58b0bc60"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""33fb07c0-aa6f-4780-9341-d5f45c9cfda7"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""9d18c254-624d-46c5-8562-81daf284a65e"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""293b824e-10d1-4e44-9954-bbf84320160e"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""f134b0b5-3fd2-47ed-a405-a59506be792e"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""120d8cca-4659-4e41-8a05-fa9d93c6d519"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Arrows"",
                    ""id"": ""accc6e60-9126-417c-aede-faa98ea6b38c"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""7789a364-0336-4ec6-a3fc-b2339a372ad3"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""5e47d7f0-2757-4a9e-a3d1-c666d50e34d6"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""c5b714bd-616a-4558-b8e2-977f0ff401b0"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""824c1015-d3c0-4f33-8a5f-2e2009893ab2"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""f5f014a7-4fea-47ff-9113-8b8a86534412"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ac1ef2da-da4b-4e99-940e-b6e42a43269b"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""58704ca8-4708-4370-a74e-313ef5b443f5"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Master
        m_Master = asset.FindActionMap("Master", throwIfNotFound: true);
        m_Master_UpgradeMenu = m_Master.FindAction("UpgradeMenu", throwIfNotFound: true);
        m_Master_Pause = m_Master.FindAction("Pause", throwIfNotFound: true);
        m_Master_Back = m_Master.FindAction("Back", throwIfNotFound: true);
        m_Master_MousePosition = m_Master.FindAction("MousePosition", throwIfNotFound: true);
        m_Master_Move = m_Master.FindAction("Move", throwIfNotFound: true);
        m_Master_Aim = m_Master.FindAction("Aim", throwIfNotFound: true);
        m_Master_Shoot = m_Master.FindAction("Shoot", throwIfNotFound: true);
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

    // Master
    private readonly InputActionMap m_Master;
    private IMasterActions m_MasterActionsCallbackInterface;
    private readonly InputAction m_Master_UpgradeMenu;
    private readonly InputAction m_Master_Pause;
    private readonly InputAction m_Master_Back;
    private readonly InputAction m_Master_MousePosition;
    private readonly InputAction m_Master_Move;
    private readonly InputAction m_Master_Aim;
    private readonly InputAction m_Master_Shoot;
    public struct MasterActions
    {
        private @Controls m_Wrapper;
        public MasterActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @UpgradeMenu => m_Wrapper.m_Master_UpgradeMenu;
        public InputAction @Pause => m_Wrapper.m_Master_Pause;
        public InputAction @Back => m_Wrapper.m_Master_Back;
        public InputAction @MousePosition => m_Wrapper.m_Master_MousePosition;
        public InputAction @Move => m_Wrapper.m_Master_Move;
        public InputAction @Aim => m_Wrapper.m_Master_Aim;
        public InputAction @Shoot => m_Wrapper.m_Master_Shoot;
        public InputActionMap Get() { return m_Wrapper.m_Master; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MasterActions set) { return set.Get(); }
        public void SetCallbacks(IMasterActions instance)
        {
            if (m_Wrapper.m_MasterActionsCallbackInterface != null)
            {
                @UpgradeMenu.started -= m_Wrapper.m_MasterActionsCallbackInterface.OnUpgradeMenu;
                @UpgradeMenu.performed -= m_Wrapper.m_MasterActionsCallbackInterface.OnUpgradeMenu;
                @UpgradeMenu.canceled -= m_Wrapper.m_MasterActionsCallbackInterface.OnUpgradeMenu;
                @Pause.started -= m_Wrapper.m_MasterActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_MasterActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_MasterActionsCallbackInterface.OnPause;
                @Back.started -= m_Wrapper.m_MasterActionsCallbackInterface.OnBack;
                @Back.performed -= m_Wrapper.m_MasterActionsCallbackInterface.OnBack;
                @Back.canceled -= m_Wrapper.m_MasterActionsCallbackInterface.OnBack;
                @MousePosition.started -= m_Wrapper.m_MasterActionsCallbackInterface.OnMousePosition;
                @MousePosition.performed -= m_Wrapper.m_MasterActionsCallbackInterface.OnMousePosition;
                @MousePosition.canceled -= m_Wrapper.m_MasterActionsCallbackInterface.OnMousePosition;
                @Move.started -= m_Wrapper.m_MasterActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_MasterActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_MasterActionsCallbackInterface.OnMove;
                @Aim.started -= m_Wrapper.m_MasterActionsCallbackInterface.OnAim;
                @Aim.performed -= m_Wrapper.m_MasterActionsCallbackInterface.OnAim;
                @Aim.canceled -= m_Wrapper.m_MasterActionsCallbackInterface.OnAim;
                @Shoot.started -= m_Wrapper.m_MasterActionsCallbackInterface.OnShoot;
                @Shoot.performed -= m_Wrapper.m_MasterActionsCallbackInterface.OnShoot;
                @Shoot.canceled -= m_Wrapper.m_MasterActionsCallbackInterface.OnShoot;
            }
            m_Wrapper.m_MasterActionsCallbackInterface = instance;
            if (instance != null)
            {
                @UpgradeMenu.started += instance.OnUpgradeMenu;
                @UpgradeMenu.performed += instance.OnUpgradeMenu;
                @UpgradeMenu.canceled += instance.OnUpgradeMenu;
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
                @Back.started += instance.OnBack;
                @Back.performed += instance.OnBack;
                @Back.canceled += instance.OnBack;
                @MousePosition.started += instance.OnMousePosition;
                @MousePosition.performed += instance.OnMousePosition;
                @MousePosition.canceled += instance.OnMousePosition;
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Aim.started += instance.OnAim;
                @Aim.performed += instance.OnAim;
                @Aim.canceled += instance.OnAim;
                @Shoot.started += instance.OnShoot;
                @Shoot.performed += instance.OnShoot;
                @Shoot.canceled += instance.OnShoot;
            }
        }
    }
    public MasterActions @Master => new MasterActions(this);
    public interface IMasterActions
    {
        void OnUpgradeMenu(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
        void OnBack(InputAction.CallbackContext context);
        void OnMousePosition(InputAction.CallbackContext context);
        void OnMove(InputAction.CallbackContext context);
        void OnAim(InputAction.CallbackContext context);
        void OnShoot(InputAction.CallbackContext context);
    }
}
