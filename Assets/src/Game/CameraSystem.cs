using System;
using UnityEngine;

using static Unity.Mathematics.math;

public class CameraSystem : GameSystem {
	private Camera _camera;

    private Vector3 _cameraRotation;
    private float   _cameraHeight = CameraMinHeight;

    private const float CameraMaxHeight = 80f;
    private const float CameraMinHeight = 5f;
    private const float CameraSpeedHeightMultiplier = 4f;

	public CameraSystem(Game game) : base(game, false) {
        Events.SubGeneral<LevelLoadedEvent>(OnLevelLoad);
	}

    public override void OnEnable() {
        Events.SubPrivate<CameraMovedEvent>("input", OnCameraMove);
        Events.SubPrivate<CameraRotatedEvent>("input", OnCameraRotate);
    }

    public override void OnDisable() {
        Events.UnsubPrivate<CameraMovedEvent>("input", OnCameraMove);
        Events.UnsubPrivate<CameraRotatedEvent>("input", OnCameraRotate);
    }

    public override void Update() {
    }

    private void OnLevelLoad(LevelLoadedEvent evnt) {
    	_camera         = evnt.Description.Camera;
        _cameraRotation = _camera.transform.rotation.eulerAngles;
    }

    private void OnCameraRotate(CameraRotatedEvent evnt) {
        var rot = _cameraRotation;

        rot.x -= evnt.Delta.y * Config.CameraSensitivity * Clock.Delta;
        rot.x =  clamp(rot.x, Config.CameraMinAngle, Config.CameraMaxAngle);

        rot.y += evnt.Delta.x * Config.CameraSensitivity * Clock.Delta;

        if      (rot.y > 180f) rot.y -= 360f;
        else if (rot.y < -180f) rot.y += 360f;

        _cameraRotation = rot;

        _camera.transform.rotation = Quaternion.Euler(_cameraRotation);
    }

    private void OnCameraMove(CameraMovedEvent evnt) {
        var pos     = _camera.transform.position;
        var forward = _camera.transform.forward;
        var right   = _camera.transform.right;
        var speed   = Config.CameraSpeed * Clock.Delta;

        speed *= lerp(1, CameraSpeedHeightMultiplier, (_cameraHeight / CameraMaxHeight));

        forward.y = 0;
        right.y   = 0;

        pos   += forward * (evnt.Delta.z * speed);
        pos   += right   * (evnt.Delta.x * speed);

        _cameraHeight += evnt.Delta.y * Config.CameraScrollSpeed * Clock.Delta;

        _cameraHeight = clamp(_cameraHeight, CameraMinHeight, CameraMaxHeight);

        pos.y = Game.GetSystem<TerrainSystem>().SampleHeight(pos) + _cameraHeight;

        _camera.transform.position = pos;
    }
}