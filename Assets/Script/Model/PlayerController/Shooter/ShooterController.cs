//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.6.1
//     from Assets/Script/Model/PlayerController/Shooter/ShooterController.inputactions
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

namespace Com.Unnamed.RacingGame.Shooter
{
    public partial class @ShooterController: IInputActionCollection2, IDisposable
    {
        public InputActionAsset asset { get; }
        public @ShooterController()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""ShooterController"",
    ""maps"": [
        {
            ""name"": ""Shooter"",
            ""id"": ""48f3611d-6ede-4a59-b6f5-25a27f21fad3"",
            ""actions"": [
                {
                    ""name"": ""AimMouse"",
                    ""type"": ""Value"",
                    ""id"": ""5e00c189-f9cd-40d5-9565-e8452bb92961"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""AimStick"",
                    ""type"": ""Value"",
                    ""id"": ""5550600b-7810-4ef9-9803-be806497794b"",
                    ""expectedControlType"": ""Stick"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""AimX"",
                    ""type"": ""Value"",
                    ""id"": ""81f405fd-b7a4-445b-9687-3c359e467728"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""AimY"",
                    ""type"": ""Value"",
                    ""id"": ""f55c16ff-8f91-4d3a-a1c5-f0f10d4565ed"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Thruster"",
                    ""type"": ""Value"",
                    ""id"": ""5c0da426-1174-4aa6-ade9-c3c92b97c48b"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""ShootPrimary"",
                    ""type"": ""Button"",
                    ""id"": ""624a0ccd-a078-450c-9af7-b8901a913ccd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ShootSecondary"",
                    ""type"": ""Button"",
                    ""id"": ""f78d7bce-1f78-4790-9c13-e1a5c6502c6c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""20394ea5-2ecc-4e26-a083-5c7022f93e69"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse/Keyboard"",
                    ""action"": ""ShootPrimary"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8e30d3a0-b24d-4d38-8c58-f723333fecf7"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""ShootPrimary"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4e9c9b9d-b5d3-4a63-b528-5484b62638d9"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse/Keyboard"",
                    ""action"": ""AimMouse"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Azimuth"",
                    ""id"": ""a0a188c3-844d-4aa5-a0d8-58a486e9091e"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AimX"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""f90962be-b95b-44a0-8d98-4b97c7e1da74"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse/Keyboard"",
                    ""action"": ""AimX"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""d15a5b2a-6ce8-4586-b026-a32e957bb121"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse/Keyboard"",
                    ""action"": ""AimX"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""4ff5eb3a-165b-4652-a0ac-fe8d1cfca091"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse/Keyboard"",
                    ""action"": ""AimX"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""4f93801b-ed3a-487a-90ef-d3ea77ac45b4"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse/Keyboard"",
                    ""action"": ""AimX"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Altitude"",
                    ""id"": ""f5d4f74d-ca2e-45e9-886b-83f56833ab75"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AimY"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""6a42c175-719f-4605-abbc-c63596041fcf"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse/Keyboard"",
                    ""action"": ""AimY"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""d122eb55-c711-4b16-976f-44937a46be71"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse/Keyboard"",
                    ""action"": ""AimY"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""e54033f2-9008-41d9-9791-6c841059d1c9"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse/Keyboard"",
                    ""action"": ""AimY"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""878cd101-53f0-45ee-bf2f-de2081bcbe03"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse/Keyboard"",
                    ""action"": ""AimY"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""102d9343-35c2-4896-92f0-1169c2cfdc2c"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""AimStick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""debf937b-6bd4-4c29-aa91-190e19fe120b"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""AimStick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""24f616e6-92d7-4388-a0e3-04011b07c29c"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse/Keyboard"",
                    ""action"": ""ShootSecondary"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d38b7ce1-c740-444c-8f89-70cf3adaf176"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""ShootSecondary"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Q/E"",
                    ""id"": ""3aca5bbb-b2a9-4076-9d0f-12d2b64b24b8"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Thruster"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""feeab1bf-ba75-4a5f-af5b-1d4b60fe9854"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse/Keyboard"",
                    ""action"": ""Thruster"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""ae7e23f2-b5a8-4d2f-a16b-95d973026be6"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse/Keyboard"",
                    ""action"": ""Thruster"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""D-pad Up/Down"",
                    ""id"": ""69443fb3-1736-433c-a6e4-786f537b7713"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Thruster"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""ec40bd2d-73fa-4bc9-a0eb-858c63d36bbd"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Thruster"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""81252519-a1ab-4d09-a6c2-abb06d9b5636"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Thruster"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Controller"",
            ""bindingGroup"": ""Controller"",
            ""devices"": [
                {
                    ""devicePath"": ""<XInputController>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Mouse/Keyboard"",
            ""bindingGroup"": ""Mouse/Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
            // Shooter
            m_Shooter = asset.FindActionMap("Shooter", throwIfNotFound: true);
            m_Shooter_AimMouse = m_Shooter.FindAction("AimMouse", throwIfNotFound: true);
            m_Shooter_AimStick = m_Shooter.FindAction("AimStick", throwIfNotFound: true);
            m_Shooter_AimX = m_Shooter.FindAction("AimX", throwIfNotFound: true);
            m_Shooter_AimY = m_Shooter.FindAction("AimY", throwIfNotFound: true);
            m_Shooter_Thruster = m_Shooter.FindAction("Thruster", throwIfNotFound: true);
            m_Shooter_ShootPrimary = m_Shooter.FindAction("ShootPrimary", throwIfNotFound: true);
            m_Shooter_ShootSecondary = m_Shooter.FindAction("ShootSecondary", throwIfNotFound: true);
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

        // Shooter
        private readonly InputActionMap m_Shooter;
        private List<IShooterActions> m_ShooterActionsCallbackInterfaces = new List<IShooterActions>();
        private readonly InputAction m_Shooter_AimMouse;
        private readonly InputAction m_Shooter_AimStick;
        private readonly InputAction m_Shooter_AimX;
        private readonly InputAction m_Shooter_AimY;
        private readonly InputAction m_Shooter_Thruster;
        private readonly InputAction m_Shooter_ShootPrimary;
        private readonly InputAction m_Shooter_ShootSecondary;
        public struct ShooterActions
        {
            private @ShooterController m_Wrapper;
            public ShooterActions(@ShooterController wrapper) { m_Wrapper = wrapper; }
            public InputAction @AimMouse => m_Wrapper.m_Shooter_AimMouse;
            public InputAction @AimStick => m_Wrapper.m_Shooter_AimStick;
            public InputAction @AimX => m_Wrapper.m_Shooter_AimX;
            public InputAction @AimY => m_Wrapper.m_Shooter_AimY;
            public InputAction @Thruster => m_Wrapper.m_Shooter_Thruster;
            public InputAction @ShootPrimary => m_Wrapper.m_Shooter_ShootPrimary;
            public InputAction @ShootSecondary => m_Wrapper.m_Shooter_ShootSecondary;
            public InputActionMap Get() { return m_Wrapper.m_Shooter; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(ShooterActions set) { return set.Get(); }
            public void AddCallbacks(IShooterActions instance)
            {
                if (instance == null || m_Wrapper.m_ShooterActionsCallbackInterfaces.Contains(instance)) return;
                m_Wrapper.m_ShooterActionsCallbackInterfaces.Add(instance);
                @AimMouse.started += instance.OnAimMouse;
                @AimMouse.performed += instance.OnAimMouse;
                @AimMouse.canceled += instance.OnAimMouse;
                @AimStick.started += instance.OnAimStick;
                @AimStick.performed += instance.OnAimStick;
                @AimStick.canceled += instance.OnAimStick;
                @AimX.started += instance.OnAimX;
                @AimX.performed += instance.OnAimX;
                @AimX.canceled += instance.OnAimX;
                @AimY.started += instance.OnAimY;
                @AimY.performed += instance.OnAimY;
                @AimY.canceled += instance.OnAimY;
                @Thruster.started += instance.OnThruster;
                @Thruster.performed += instance.OnThruster;
                @Thruster.canceled += instance.OnThruster;
                @ShootPrimary.started += instance.OnShootPrimary;
                @ShootPrimary.performed += instance.OnShootPrimary;
                @ShootPrimary.canceled += instance.OnShootPrimary;
                @ShootSecondary.started += instance.OnShootSecondary;
                @ShootSecondary.performed += instance.OnShootSecondary;
                @ShootSecondary.canceled += instance.OnShootSecondary;
            }

            private void UnregisterCallbacks(IShooterActions instance)
            {
                @AimMouse.started -= instance.OnAimMouse;
                @AimMouse.performed -= instance.OnAimMouse;
                @AimMouse.canceled -= instance.OnAimMouse;
                @AimStick.started -= instance.OnAimStick;
                @AimStick.performed -= instance.OnAimStick;
                @AimStick.canceled -= instance.OnAimStick;
                @AimX.started -= instance.OnAimX;
                @AimX.performed -= instance.OnAimX;
                @AimX.canceled -= instance.OnAimX;
                @AimY.started -= instance.OnAimY;
                @AimY.performed -= instance.OnAimY;
                @AimY.canceled -= instance.OnAimY;
                @Thruster.started -= instance.OnThruster;
                @Thruster.performed -= instance.OnThruster;
                @Thruster.canceled -= instance.OnThruster;
                @ShootPrimary.started -= instance.OnShootPrimary;
                @ShootPrimary.performed -= instance.OnShootPrimary;
                @ShootPrimary.canceled -= instance.OnShootPrimary;
                @ShootSecondary.started -= instance.OnShootSecondary;
                @ShootSecondary.performed -= instance.OnShootSecondary;
                @ShootSecondary.canceled -= instance.OnShootSecondary;
            }

            public void RemoveCallbacks(IShooterActions instance)
            {
                if (m_Wrapper.m_ShooterActionsCallbackInterfaces.Remove(instance))
                    UnregisterCallbacks(instance);
            }

            public void SetCallbacks(IShooterActions instance)
            {
                foreach (var item in m_Wrapper.m_ShooterActionsCallbackInterfaces)
                    UnregisterCallbacks(item);
                m_Wrapper.m_ShooterActionsCallbackInterfaces.Clear();
                AddCallbacks(instance);
            }
        }
        public ShooterActions @Shooter => new ShooterActions(this);
        private int m_ControllerSchemeIndex = -1;
        public InputControlScheme ControllerScheme
        {
            get
            {
                if (m_ControllerSchemeIndex == -1) m_ControllerSchemeIndex = asset.FindControlSchemeIndex("Controller");
                return asset.controlSchemes[m_ControllerSchemeIndex];
            }
        }
        private int m_MouseKeyboardSchemeIndex = -1;
        public InputControlScheme MouseKeyboardScheme
        {
            get
            {
                if (m_MouseKeyboardSchemeIndex == -1) m_MouseKeyboardSchemeIndex = asset.FindControlSchemeIndex("Mouse/Keyboard");
                return asset.controlSchemes[m_MouseKeyboardSchemeIndex];
            }
        }
        public interface IShooterActions
        {
            void OnAimMouse(InputAction.CallbackContext context);
            void OnAimStick(InputAction.CallbackContext context);
            void OnAimX(InputAction.CallbackContext context);
            void OnAimY(InputAction.CallbackContext context);
            void OnThruster(InputAction.CallbackContext context);
            void OnShootPrimary(InputAction.CallbackContext context);
            void OnShootSecondary(InputAction.CallbackContext context);
        }
    }
}
