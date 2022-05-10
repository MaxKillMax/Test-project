using UnityEngine;

public class ContinueButton : MonoBehaviour
{
    [SerializeField] private TimeManager _timeManager;

    public void OnContinueSetted()
    {
        _timeManager.SwitchPause();
    }
}
