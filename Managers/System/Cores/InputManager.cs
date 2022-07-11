using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{ 
    public delegate void MovementMethod(Vector3 dir);
    public event MovementMethod MovementEventHandler = null;
    public void Joystick(Vector3 dir) => MovementEventHandler?.Invoke(dir);
}