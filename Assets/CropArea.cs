using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropArea : MonoBehaviour {
	// Crop area keeps track of the plots that are contained within it 

	// It also keeps track of the objects that are placed on the plots

	public static Action OnCropAreaPlaced;
	private void Awake() {
		OnCropAreaPlaced?.Invoke();
	}

	private void Start() {

	}

	private void Update() {
		// Loop through all of the crop slots and see if they need to be grown
	}
}
