using UnityEngine;
using System;

public class TimeManager : MonoBehaviour
{
    public event Action onGamePaused;
    public event Action onGameUnpaused;

    private bool _onPause = false;

    public bool onPause => _onPause;

    public void SwitchPause()
    {
        _onPause = !_onPause;

        if (_onPause)
            onGamePaused?.Invoke();
        else
            onGameUnpaused?.Invoke();
    }
}
