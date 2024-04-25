using System;
using UnityEngine;

public class ClickableObject : MonoBehaviour {
	public event EventHandler OnClick;

	public void Clicked() {
		OnClick?.Invoke(this, EventArgs.Empty);
	}
}
