using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
        
    [SerializeField] private float offsetY = 45f;
    [SerializeField] private float offsetZ = -40f;
    [SerializeField] private float offsetX = 0f;

    private StageManager _stageManager;
    private Vector3 _cameraPosition;
    private bool _isStart;
        
    private void Start()
    {
        _isStart = false;
        _stageManager = GameObject.Find(nameof(StageManager)).GetComponent<StageManager>();
        _stageManager.OnStageStart += Init;
        FovSet();
    }
        
    private void LateUpdate()
    {
        if (_isStart is false)
        {
            return;
        }
        
        var position = player.transform.position;

        _cameraPosition.x = position.x + offsetX;
        _cameraPosition.y = position.y + offsetY;
        _cameraPosition.z = position.z + offsetZ;

        transform.position = _cameraPosition;
    }
        
    private void Init(StageManager owner)
    {
        player = owner.GetPlayer();
        _isStart = true;
    }

    private static void FovSet()
    {
        var cam = UnityEngine.Camera.main;

        float width = Screen.width;
        float height = Screen.height;

        const float displayRatio = 16.0f / 9.0f;
        var deviceRatio = height / width;

        var fov = 0f;
            
        if (cam != null)
        {
            fov = cam.fieldOfView;
        }

        var newFov = deviceRatio * fov / displayRatio;

    }
}
