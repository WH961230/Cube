//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.5.1
//     from Assets/LazyPan/Bundles/Configs/Input/LazyPanInputControl.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @LazyPanInputControl: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @LazyPanInputControl()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""LazyPanInputControl"",
    ""maps"": [
        {
            ""name"": ""Global"",
            ""id"": ""c3c67cab-61c0-4b12-933c-10796638e97a"",
            ""actions"": [
                {
                    ""name"": ""Space"",
                    ""type"": ""Button"",
                    ""id"": ""125b0dc6-02a0-4f54-aa98-285f8ce0c8c1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Console"",
                    ""type"": ""Button"",
                    ""id"": ""c0040cd7-2202-4fb4-a812-b94fc7e4f445"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Escape"",
                    ""type"": ""Button"",
                    ""id"": ""09cef4c5-4939-4944-94cb-0e7314b66417"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""R"",
                    ""type"": ""Button"",
                    ""id"": ""c732785d-fe27-46d1-a860-5c3aef11bf0b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Q"",
                    ""type"": ""Button"",
                    ""id"": ""fb70b2cb-44f2-4ffd-89c0-273514d729a4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""E"",
                    ""type"": ""Button"",
                    ""id"": ""086470f1-2e7f-4e41-b68c-599b5c577158"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""C"",
                    ""type"": ""Button"",
                    ""id"": ""01a870b9-c9a2-490e-af17-e0eccae5c24b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""T"",
                    ""type"": ""Button"",
                    ""id"": ""e325f6b5-2d62-4487-8885-dda3cf928bd0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""M"",
                    ""type"": ""Button"",
                    ""id"": ""e349474f-0055-49d6-932f-88d10ff57012"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""7bf4b638-02eb-4401-bb8d-bb1200ece0ae"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Space"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cdbd83c5-22c1-4f4e-b815-bda86cb9cfa5"",
                    ""path"": ""<XInputController>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Space"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""001373cc-65c2-45dd-9267-40883260151e"",
                    ""path"": ""<XInputController>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Space"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""53e77e4d-fb7a-45a6-80c8-22ef36d9dc02"",
                    ""path"": ""<Keyboard>/backquote"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Console"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ebc553f7-e70f-42a1-b40b-4e48e18817f7"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Escape"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2b3d27ac-fc28-4139-ae5e-6adbe64d4e91"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""R"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dadf6569-e35f-4bb5-ad03-933bb34d56b0"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Q"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fdead6ff-9901-46b4-a6b0-5f6a0f41384c"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""E"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c4caa38f-f9d7-4d65-a9ab-c1f8d94d0c8c"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""C"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a01e2615-f638-44b8-a8e1-35c369f72f4f"",
                    ""path"": ""<Keyboard>/t"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""T"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""06583966-27dd-4087-af4b-4d0c1a651bd5"",
                    ""path"": ""<Keyboard>/m"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""M"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Player"",
            ""id"": ""041603f5-47f1-4d07-8f34-416f43ff15e7"",
            ""actions"": [
                {
                    ""name"": ""Motion"",
                    ""type"": ""Value"",
                    ""id"": ""2330f3c6-0aa2-42b2-9d48-fe0a98bd95cc"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""2c2a6a68-7bab-4b63-962f-2bdfe5f7d787"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Motion"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""6627a07b-d118-4bd1-af79-d3885ec6036f"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Motion"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""8e9d8b14-29fe-4b10-abb1-e34a03871872"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Motion"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""2b111c5e-d2ee-4d96-84d5-7ae268fee50c"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Motion"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""be15fffd-7017-4028-9d19-eec57ffb1ed7"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Motion"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""fa9fedde-748b-4e13-8dc5-2d5fc8383ccc"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Motion"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""1a22e81b-ecf5-4a2c-8f37-1f1bbf332fc2"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Motion"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""bc9298b4-b8c6-4c60-b49f-eb21efcc9620"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Motion"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""debadc9e-d0da-44c5-b24c-f95b4fb42967"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Motion"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""79c63362-7df2-4ace-9ae5-30d7b74b9402"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Motion"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Global
        m_Global = asset.FindActionMap("Global", throwIfNotFound: true);
        m_Global_Space = m_Global.FindAction("Space", throwIfNotFound: true);
        m_Global_Console = m_Global.FindAction("Console", throwIfNotFound: true);
        m_Global_Escape = m_Global.FindAction("Escape", throwIfNotFound: true);
        m_Global_R = m_Global.FindAction("R", throwIfNotFound: true);
        m_Global_Q = m_Global.FindAction("Q", throwIfNotFound: true);
        m_Global_E = m_Global.FindAction("E", throwIfNotFound: true);
        m_Global_C = m_Global.FindAction("C", throwIfNotFound: true);
        m_Global_T = m_Global.FindAction("T", throwIfNotFound: true);
        m_Global_M = m_Global.FindAction("M", throwIfNotFound: true);
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Motion = m_Player.FindAction("Motion", throwIfNotFound: true);
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

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Global
    private readonly InputActionMap m_Global;
    private List<IGlobalActions> m_GlobalActionsCallbackInterfaces = new List<IGlobalActions>();
    private readonly InputAction m_Global_Space;
    private readonly InputAction m_Global_Console;
    private readonly InputAction m_Global_Escape;
    private readonly InputAction m_Global_R;
    private readonly InputAction m_Global_Q;
    private readonly InputAction m_Global_E;
    private readonly InputAction m_Global_C;
    private readonly InputAction m_Global_T;
    private readonly InputAction m_Global_M;
    public struct GlobalActions
    {
        private @LazyPanInputControl m_Wrapper;
        public GlobalActions(@LazyPanInputControl wrapper) { m_Wrapper = wrapper; }
        public InputAction @Space => m_Wrapper.m_Global_Space;
        public InputAction @Console => m_Wrapper.m_Global_Console;
        public InputAction @Escape => m_Wrapper.m_Global_Escape;
        public InputAction @R => m_Wrapper.m_Global_R;
        public InputAction @Q => m_Wrapper.m_Global_Q;
        public InputAction @E => m_Wrapper.m_Global_E;
        public InputAction @C => m_Wrapper.m_Global_C;
        public InputAction @T => m_Wrapper.m_Global_T;
        public InputAction @M => m_Wrapper.m_Global_M;
        public InputActionMap Get() { return m_Wrapper.m_Global; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GlobalActions set) { return set.Get(); }
        public void AddCallbacks(IGlobalActions instance)
        {
            if (instance == null || m_Wrapper.m_GlobalActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_GlobalActionsCallbackInterfaces.Add(instance);
            @Space.started += instance.OnSpace;
            @Space.performed += instance.OnSpace;
            @Space.canceled += instance.OnSpace;
            @Console.started += instance.OnConsole;
            @Console.performed += instance.OnConsole;
            @Console.canceled += instance.OnConsole;
            @Escape.started += instance.OnEscape;
            @Escape.performed += instance.OnEscape;
            @Escape.canceled += instance.OnEscape;
            @R.started += instance.OnR;
            @R.performed += instance.OnR;
            @R.canceled += instance.OnR;
            @Q.started += instance.OnQ;
            @Q.performed += instance.OnQ;
            @Q.canceled += instance.OnQ;
            @E.started += instance.OnE;
            @E.performed += instance.OnE;
            @E.canceled += instance.OnE;
            @C.started += instance.OnC;
            @C.performed += instance.OnC;
            @C.canceled += instance.OnC;
            @T.started += instance.OnT;
            @T.performed += instance.OnT;
            @T.canceled += instance.OnT;
            @M.started += instance.OnM;
            @M.performed += instance.OnM;
            @M.canceled += instance.OnM;
        }

        private void UnregisterCallbacks(IGlobalActions instance)
        {
            @Space.started -= instance.OnSpace;
            @Space.performed -= instance.OnSpace;
            @Space.canceled -= instance.OnSpace;
            @Console.started -= instance.OnConsole;
            @Console.performed -= instance.OnConsole;
            @Console.canceled -= instance.OnConsole;
            @Escape.started -= instance.OnEscape;
            @Escape.performed -= instance.OnEscape;
            @Escape.canceled -= instance.OnEscape;
            @R.started -= instance.OnR;
            @R.performed -= instance.OnR;
            @R.canceled -= instance.OnR;
            @Q.started -= instance.OnQ;
            @Q.performed -= instance.OnQ;
            @Q.canceled -= instance.OnQ;
            @E.started -= instance.OnE;
            @E.performed -= instance.OnE;
            @E.canceled -= instance.OnE;
            @C.started -= instance.OnC;
            @C.performed -= instance.OnC;
            @C.canceled -= instance.OnC;
            @T.started -= instance.OnT;
            @T.performed -= instance.OnT;
            @T.canceled -= instance.OnT;
            @M.started -= instance.OnM;
            @M.performed -= instance.OnM;
            @M.canceled -= instance.OnM;
        }

        public void RemoveCallbacks(IGlobalActions instance)
        {
            if (m_Wrapper.m_GlobalActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IGlobalActions instance)
        {
            foreach (var item in m_Wrapper.m_GlobalActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_GlobalActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public GlobalActions @Global => new GlobalActions(this);

    // Player
    private readonly InputActionMap m_Player;
    private List<IPlayerActions> m_PlayerActionsCallbackInterfaces = new List<IPlayerActions>();
    private readonly InputAction m_Player_Motion;
    public struct PlayerActions
    {
        private @LazyPanInputControl m_Wrapper;
        public PlayerActions(@LazyPanInputControl wrapper) { m_Wrapper = wrapper; }
        public InputAction @Motion => m_Wrapper.m_Player_Motion;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Add(instance);
            @Motion.started += instance.OnMotion;
            @Motion.performed += instance.OnMotion;
            @Motion.canceled += instance.OnMotion;
        }

        private void UnregisterCallbacks(IPlayerActions instance)
        {
            @Motion.started -= instance.OnMotion;
            @Motion.performed -= instance.OnMotion;
            @Motion.canceled -= instance.OnMotion;
        }

        public void RemoveCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    public interface IGlobalActions
    {
        void OnSpace(InputAction.CallbackContext context);
        void OnConsole(InputAction.CallbackContext context);
        void OnEscape(InputAction.CallbackContext context);
        void OnR(InputAction.CallbackContext context);
        void OnQ(InputAction.CallbackContext context);
        void OnE(InputAction.CallbackContext context);
        void OnC(InputAction.CallbackContext context);
        void OnT(InputAction.CallbackContext context);
        void OnM(InputAction.CallbackContext context);
    }
    public interface IPlayerActions
    {
        void OnMotion(InputAction.CallbackContext context);
    }
}
