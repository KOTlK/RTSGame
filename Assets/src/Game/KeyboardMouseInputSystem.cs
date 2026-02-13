using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeyboardMouseInputSystem : GameSystem {
	private InputAction _cameraMove;
	private InputAction _cameraRotation;
	private InputAction _cameraRotationButton;
	private InputAction _cameraRotationButtons;

	public KeyboardMouseInputSystem(Game game) : base(game, true) {
	}

	public override void OnEnable() {
		_cameraMove     	   = InputSystem.actions.FindAction("CameraMove");
		_cameraRotation 	   = InputSystem.actions.FindAction("CameraRotation");
		_cameraRotationButton  = InputSystem.actions.FindAction("RotatingCamera");
		_cameraRotationButtons = InputSystem.actions.FindAction("CameraRotationButtons");
	}

	public override void OnDisable() {
	}

	public override void Update() {
		var move = _cameraMove.ReadValue<Vector3>();

		if (move != Vector3.zero) {
			var evnt  = new CameraMovedEvent();

	    	evnt.Delta = move;

	    	Events.RaisePrivate("input", evnt);
		}

		if (_cameraRotationButtons.phase == InputActionPhase.Started) {
			var value = _cameraRotationButtons.ReadValue<Vector2>();
			value.x *= Config.CameraHorizontalRotationButtonSpeed;
			value.y *= Config.CameraVerticalRotationButtonSpeed;
	    	var evnt  = new CameraRotatedEvent();

	    	evnt.Delta = value;

	    	Events.RaisePrivate("input", evnt);
		} else if (_cameraRotationButton.phase == InputActionPhase.Performed) {
			var value = _cameraRotation.ReadValue<Vector2>();
	    	var evnt  = new CameraRotatedEvent();

	    	evnt.Delta = value;

	    	Events.RaisePrivate("input", evnt);
		}
	}
}