using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    #region Managers

    private readonly InputManager _input = new InputManager();
    private readonly PoolingManager _pooling = new PoolingManager();
    private readonly SoundManager _sound = new SoundManager();
    private readonly UIManager _ui = new UIManager();
    public static InputManager Input => Instance._input;
    public static PoolingManager Pooling => Instance._pooling;
    public static SoundManager Sound => Instance._sound;
    
    public static UIManager UI => Instance._ui;
    
    #endregion

    public void ManagerInit()
    {
        _pooling.Init(100);
        _sound.Init(100);
        _ui.Init();
    }
}
