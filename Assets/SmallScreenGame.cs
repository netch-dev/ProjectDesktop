// 2024-05-06 AI-Tag 
// This was created with assistance from Muse, a Unity Artificial Intelligence product

using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class SmallScreenGame : MonoBehaviour {
	public bool isAlwaysOnTop = true;

#if UNITY_EDITOR

#else
	[DllImport("user32.dll")]
	public static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

	[DllImport("user32.dll")]
	public static extern bool SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter, int x, int y, int width, int height, uint flags);

	[DllImport("user32.dll")]
	public static extern IntPtr GetActiveWindow();

	private const int GWL_STYLE = -16;
	private const int WS_POPUP = unchecked((int)0x80000000L);
	private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
	private static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);


	private void Start() {
		IntPtr window = GetActiveWindow();

		// Change the style of the window to remove the border and title bar
		SetWindowLong(window, GWL_STYLE, WS_POPUP);

		// Resize and position the window
		int width = Screen.currentResolution.width;
		int height = Screen.currentResolution.height;
		//SetWindowPos(window, isAlwaysOnTop ? HWND_TOPMOST : HWND_NOTOPMOST, 0, 3 * height / 4, width, height / 4, 0x0040);
		SetWindowPos(window, HWND_TOPMOST, 0, 3 * height / 4, width, height / 4, 0x0040);
	}
#endif
}
