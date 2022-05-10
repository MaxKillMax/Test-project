using UnityEngine;

public class PauseButton : MonoBehaviour
{
    [SerializeField] private TimeManager _timeManager;

    public void OnPauseSetted()
    {
        _timeManager.SwitchPause();
    }
}
