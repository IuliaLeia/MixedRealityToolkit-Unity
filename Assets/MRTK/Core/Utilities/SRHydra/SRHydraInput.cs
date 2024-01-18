using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Runtime.InteropServices;

public class SRHydraInput : MonoBehaviour
{
    public static Vector3 CurrentMouse = new Vector3();
    static bool[] PressedKeys = new bool[254];
    static bool[] PreviouslyPressedKeys = new bool[254];

    [DllImport("user32.dll")]
    private static extern short GetAsyncKeyState(int vKey);

    [DllImport("SRHydraClient.dll")]
    private static extern bool xrGetAsyncKeyState(int vKey);

    static int keySource;

    [DllImport("SRHydraClient.dll")]
    private static extern float xrGetMousePosition(int index);

    public static bool GetKey(int vKey)
    {
        return PressedKeys[vKey];
    }

   public static bool GetKey(KeyCode vKey)
    {
        int virtualKey = keyCodeToVirtualKey[vKey];
        return GetKey(virtualKey);
    }

    public static bool GetKeyDown(int vKey)
    {
        return !PreviouslyPressedKeys[vKey] && PressedKeys[vKey];
    }

    public static bool GetKeyUp(int vKey)
    {
        return PreviouslyPressedKeys[vKey] && !PressedKeys[vKey];
    }

    public static bool GetKeyDown(KeyCode vKey)
    {
        int virtualKey = keyCodeToVirtualKey[vKey];
        return GetKeyDown(virtualKey);
    }

    public static bool GetKeyUp(KeyCode vKey)
    {
        int virtualKey = keyCodeToVirtualKey[vKey];
        return GetKeyUp(virtualKey);
    }

    private static Dictionary<KeyCode, int> keyCodeToVirtualKey = new Dictionary<KeyCode, int>()
    {
        { KeyCode.None, 0 },

        // Alphanumeric keys
        { KeyCode.A, 0x41 }, { KeyCode.B, 0x42 }, { KeyCode.C, 0x43 }, { KeyCode.D, 0x44 }, { KeyCode.E, 0x45 },
        { KeyCode.F, 0x46 }, { KeyCode.G, 0x47 }, { KeyCode.H, 0x48 }, { KeyCode.I, 0x49 }, { KeyCode.J, 0x4A },
        { KeyCode.K, 0x4B }, { KeyCode.L, 0x4C }, { KeyCode.M, 0x4D }, { KeyCode.N, 0x4E }, { KeyCode.O, 0x4F },
        { KeyCode.P, 0x50 }, { KeyCode.Q, 0x51 }, { KeyCode.R, 0x52 }, { KeyCode.S, 0x53 }, { KeyCode.T, 0x54 },
        { KeyCode.U, 0x55 }, { KeyCode.V, 0x56 }, { KeyCode.W, 0x57 }, { KeyCode.X, 0x58 }, { KeyCode.Y, 0x59 },
        { KeyCode.Z, 0x5A },

        // Numeric keys
        { KeyCode.Alpha0, 0x30 }, { KeyCode.Alpha1, 0x31 }, { KeyCode.Alpha2, 0x32 }, { KeyCode.Alpha3, 0x33 },
        { KeyCode.Alpha4, 0x34 }, { KeyCode.Alpha5, 0x35 }, { KeyCode.Alpha6, 0x36 }, { KeyCode.Alpha7, 0x37 },
        { KeyCode.Alpha8, 0x38 }, { KeyCode.Alpha9, 0x39 },

        // Special keys
        { KeyCode.Space, 0x20 }, { KeyCode.Return, 0x0D }, { KeyCode.Escape, 0x1B }, { KeyCode.Backspace, 0x08 },
        { KeyCode.Tab, 0x09 }, { KeyCode.CapsLock, 0x14 }, { KeyCode.LeftShift, 0xA0 }, { KeyCode.RightShift, 0xA1 },
        { KeyCode.LeftControl, 0xA2 }, { KeyCode.RightControl, 0xA3 }, { KeyCode.LeftAlt, 0xA4 }, { KeyCode.RightAlt, 0xA5 },
        { KeyCode.LeftCommand, 0x5B }, { KeyCode.RightCommand, 0x5C }, { KeyCode.LeftWindows, 0x5B }, { KeyCode.RightWindows, 0x5C },

        // Arrow keys
        { KeyCode.UpArrow, 0x26 }, { KeyCode.DownArrow, 0x28 }, { KeyCode.LeftArrow, 0x25 }, { KeyCode.RightArrow, 0x27 },

        // Function keys
        { KeyCode.F1, 0x70 }, { KeyCode.F2, 0x71 }, { KeyCode.F3, 0x72 }, { KeyCode.F4, 0x73 }, { KeyCode.F5, 0x74 },
        { KeyCode.F6, 0x75 }, { KeyCode.F7, 0x76 }, { KeyCode.F8, 0x77 }, { KeyCode.F9, 0x78 }, { KeyCode.F10, 0x79 },
        { KeyCode.F11, 0x7A }, { KeyCode.F12, 0x7B },

        // Numeric keypad keys
        { KeyCode.Keypad0, 0x60 }, { KeyCode.Keypad1, 0x61 }, { KeyCode.Keypad2, 0x62 }, { KeyCode.Keypad3, 0x63 },
        { KeyCode.Keypad4, 0x64 }, { KeyCode.Keypad5, 0x65 }, { KeyCode.Keypad6, 0x66 }, { KeyCode.Keypad7, 0x67 },
        { KeyCode.Keypad8, 0x68 }, { KeyCode.Keypad9, 0x69 }, { KeyCode.KeypadDivide, 0x6F }, { KeyCode.KeypadMultiply, 0x6A },
        { KeyCode.KeypadMinus, 0x6D }, { KeyCode.KeypadPlus, 0x6B }, { KeyCode.KeypadEnter, 0x0D }, { KeyCode.KeypadPeriod, 0x6E },
    };

    // Update is called once per frame
    void Update()
    {
        Vector3 HydraMouse = new Vector3();
        Vector3 UnityMouse = new Vector3();

        // Preparing mouse position input for raycast in viewport
        float scaledUnityMousePositionX = xrGetMousePosition(0);
        float scaledUnityMousePositionY = (1 - xrGetMousePosition(1));

        HydraMouse = new Vector3(scaledUnityMousePositionX, scaledUnityMousePositionY, 0);

        //Unity Editor mouse input normalization (0-1 range)
        UnityMouse.x = Input.mousePosition.x / Screen.width;
        UnityMouse.y = Input.mousePosition.y / Screen.height;

        //Check if the Mouse cursor is focusing on Unity or Hydra window
        if ((UnityMouse.x < 0.0f || UnityMouse.y < 0.0f) || (UnityMouse.x > 1.0f || UnityMouse.y > 1.0f))
        {
            UnityMouse = HydraMouse;
            CurrentMouse = HydraMouse;
            keySource = 1; //this variable indicates the window source target from where the Keyboard key is hit
        }
        else
        {
            if (HydraMouse == CurrentMouse)
            {
                CurrentMouse = UnityMouse;
            }
            keySource = 0;

        } 

        for (int i = 0; i < PressedKeys.Length; i++)
        {
            PreviouslyPressedKeys[i] = PressedKeys[i];
            
            if (keySource == 0) //Unity ecosystem
            {
                PressedKeys[i] = (GetAsyncKeyState(i) & 0x8000) != 0;
            }
            else if (keySource == 1) //Hydra ecosystem
            {
                PressedKeys[i] = xrGetAsyncKeyState(i);
            }
        }

    }
}
