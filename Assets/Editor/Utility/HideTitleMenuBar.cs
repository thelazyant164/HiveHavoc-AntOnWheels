#if UNITY_EDITOR
using System;
using System.Runtime.InteropServices;
using System.Text;
using UnityEditor;
using UnityEngine;

public static class HideTitleMenuBar
{
    [DllImport("user32.dll")]
    static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll")]
    static extern IntPtr GetMenu(IntPtr hwnd);

    [DllImport("user32.dll")]
    static extern IntPtr SetMenu(IntPtr hwnd, IntPtr hMenu);

    [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

    [DllImport("user32.dll")]
    static extern int GetWindowTextLength(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll")]
    private static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

    const string idLastMenuPtr = "ks_toggleTitleBar_lastMenuPtr";
    const long titlebarStyle = 0x00C00000;

    [MenuItem("Tools/Kamyker/Toggle title bar _`")]
    static void ToggleTitleBar()
    {
        var unity = GetActiveWindow();
        if (unity == IntPtr.Zero)
            return;

        int titleLength = GetWindowTextLength(unity);
        var title = new StringBuilder(titleLength);
        GetWindowText(unity, title, titleLength);

        if (!title.ToString().Contains("unity", StringComparison.InvariantCultureIgnoreCase))
        {
            Debug.LogError($"Can't hide title bar of {title.ToString()}, change 'unity' in the code if it's translated differently");
            return;
        }

        var menu = GetMenu(unity);
        var style = GetWindowLongPtr(unity, -16);

        if (menu != IntPtr.Zero)
        {
            PlayerPrefs.SetString(idLastMenuPtr, menu.ToString());
            SetWindowLongPtr(unity, -16, new IntPtr(style.ToInt64() - titlebarStyle));
            SetMenu(unity, IntPtr.Zero);
        }
        else
        {
            var lastMenuPtr = long.Parse(PlayerPrefs.GetString(idLastMenuPtr));
            SetWindowLongPtr(unity, -16, new IntPtr(style.ToInt64() + titlebarStyle));
            SetMenu(unity, new IntPtr(lastMenuPtr));
        }
    }
}
#endif