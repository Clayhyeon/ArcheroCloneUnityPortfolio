using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{
    public delegate void MovementMethod(Types.State state,  Vector3 dir);

    public event MovementMethod MovementEventHandler = null;
    
    public void Joystick(Vector3 dir)
    {
        var state = dir == Vector3.zero ? Types.State.Idle : Types.State.Movement;
        MovementEventHandler?.Invoke(state, dir);
    }
    
}
