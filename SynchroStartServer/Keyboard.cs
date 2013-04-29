using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SynchroStartServer
{
    public class Keyboard
    {
        public static void PressKey(char ch)
        {
            KeyDown(ch);
            KeyUp(ch);
        }
        public static void LShiftDown()
        {
            byte scanCode = (byte)WindowsAPI.MapVirtualKey((uint)VK.LSHIFT, 0);
            WindowsAPI.keybd_event((byte)VK.LSHIFT, scanCode, 0, UIntPtr.Zero);
        }
        public static void LShiftUp()
        {
            byte scanCode = (byte)WindowsAPI.MapVirtualKey((uint)VK.LSHIFT, 0);
            WindowsAPI.keybd_event((byte)VK.LSHIFT, scanCode, WindowsAPI.KEYEVENTF_KEYUP, UIntPtr.Zero);
        }
        public static void LCtrlDown()
        {
            byte scanCode = (byte)WindowsAPI.MapVirtualKey((uint)VK.LCONTROL, 0);
            WindowsAPI.keybd_event((byte)VK.LCONTROL, scanCode, 0, UIntPtr.Zero);
        }
        public static void LCtrlUp()
        {
            byte scanCode = (byte)WindowsAPI.MapVirtualKey((uint)VK.LCONTROL, 0);
            WindowsAPI.keybd_event((byte)VK.LCONTROL, scanCode, WindowsAPI.KEYEVENTF_KEYUP, UIntPtr.Zero);
        }
        public static void LAltDown()
        {
            byte scanCode = (byte)WindowsAPI.MapVirtualKey((uint)VK.MENU, 0);
            WindowsAPI.keybd_event((byte)VK.MENU, scanCode, 0, UIntPtr.Zero);
        }
        public static void LAltUp()
        {
            byte scanCode = (byte)WindowsAPI.MapVirtualKey((uint)VK.LCONTROL, 0);
            WindowsAPI.keybd_event((byte)VK.LCONTROL, scanCode, WindowsAPI.KEYEVENTF_KEYUP, UIntPtr.Zero);
        }
        public static void KeyDown(char ch)
        {

            INPUT[] inputs = new INPUT[1];
            inputs[0].type = WindowsAPI.INPUT_KEYBOARD;
            inputs[0].ki.dwFlags = WindowsAPI.KEYEVENTF_SCANCODE;

            byte vk = WindowsAPI.VkKeyScan(ch);
            ushort scanCode = (ushort)WindowsAPI.MapVirtualKey(vk, 0);

            inputs[0].ki.wScan = (ushort)(scanCode & 0xff);

            uint intReturn = WindowsAPI.SendInput(1, inputs, System.Runtime.InteropServices.Marshal.SizeOf(inputs[0]));
            if (intReturn != 1)
            {
                throw new Exception("Could not send key: " + scanCode);
            }
        }

        public static void KeyUp(char ch)
        {
            INPUT[] inputs = new INPUT[1];
            inputs[0].type = WindowsAPI.INPUT_KEYBOARD;
            byte vk = WindowsAPI.VkKeyScan(ch);
            ushort scanCode = (ushort)WindowsAPI.MapVirtualKey(vk, 0);
            inputs[0].ki.wScan = scanCode;
            inputs[0].ki.dwFlags = WindowsAPI.KEYEVENTF_KEYUP | WindowsAPI.KEYEVENTF_SCANCODE;
            uint intReturn = WindowsAPI.SendInput(1, inputs, System.Runtime.InteropServices.Marshal.SizeOf(inputs[0]));
            if (intReturn != 1)
            {
                throw new Exception("Could not send key: " + scanCode);
            }
        }
    }

}
