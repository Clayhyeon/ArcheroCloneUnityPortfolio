using UnityEngine;
using UnityEngine.EventSystems;


public class JoystickController : MonoBehaviour
{
    private GameObject _joystickBig;
    private GameObject _joystickSmall;

    private Vector3 _stickFirstPos;
    private Vector3 _joyVec;
    private float _radius;

    private Vector3 _joystickBigFirstPos;
    private Vector3 _joystickSmallFirstPos;

    private bool _isDrag;

    private PointerEventData _data;

    private void Start()
    {
        _isDrag = false;

        foreach (var child in GetComponentsInChildren<Transform>())
        {
            if (child.name != "Joystick Base")
            {
                continue;
            }
            _joystickBig = child.gameObject;
            _joystickSmall = _joystickBig.transform.GetChild(0).gameObject;
            break;
        }

        _radius = _joystickBig.GetComponent<RectTransform>().sizeDelta.y * 0.35f;
        _joystickBigFirstPos = _joystickBig.transform.position;
        _joystickSmallFirstPos = _joystickSmall.transform.position;
    }

    private void Update()
    {
        if (_isDrag)
        {
            OnDrag();
        }
    }

    public void PointDown()
    {
        (_joystickBig.transform.position, _stickFirstPos) = (Input.mousePosition, Input.mousePosition);
    }

    public void Drag(BaseEventData baseEventData)
    {
        _data = baseEventData as PointerEventData;
        if (!_isDrag)
        {
            _isDrag = true;
        }
    }

    private void OnDrag()
    {
        if (_data is null)
        {
            return;
        }
        
        Vector3 dragPosition = _data.position;
        _joyVec = (dragPosition - _stickFirstPos).normalized;

        var stickDistance = Vector3.Distance(dragPosition, _stickFirstPos);

        if (stickDistance < _radius)
        {
            _joystickSmall.transform.position = _stickFirstPos + _joyVec * stickDistance;
        }
        else
        {
            _joystickSmall.transform.position = _stickFirstPos + _joyVec * _radius;
        }
        
        GameManager.Input.Joystick(_joyVec);
    }

    public void Drop()
    {
        _joyVec = Vector3.zero;
        
        (_joystickBig.transform.position, _joystickSmall.transform.position) =
            (_joystickBigFirstPos, _joystickSmallFirstPos);
        
        _isDrag = false;
        GameManager.Input.Joystick(_joyVec);
    }
}