using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
    [SerializeField] private TimeManager _timeManager;

    [SerializeField] private GameObject _gameUI;
    [SerializeField] private GameObject _pauseUI;
    [SerializeField] private Image _shadow;

    private float _shadowTime = 0.3f;
    private float _onShadowOn = 0.6f;
    private float _onShadowOff = 0;
    private bool _shadowState = false;

    private void Awake()
    {
        _timeManager.onGamePaused += OpenPauseUI;
        _timeManager.onGameUnpaused += OpenGameUI;
    }

    private void OnDestroy()
    {
        _timeManager.onGamePaused -= OpenPauseUI;
        _timeManager.onGameUnpaused -= OpenGameUI;
    }

    public void QuitGame() => Application.Quit();

    public void OpenGameUI()
    {
        _gameUI.SetActive(true);
        _pauseUI.SetActive(false);

        SwitchShadow(false);
    }

    public void OpenPauseUI()
    {
        _pauseUI.SetActive(true);
        _gameUI.SetActive(false);

        SwitchShadow(true);
    }

    private void SwitchShadow(bool state)
    {
        if (state == _shadowState)
            return;

        _shadowState = state;

        _shadow.gameObject.SetActive(_shadowState);
        _shadow.DOFade(_shadowState ? _onShadowOn : _onShadowOff, _shadowTime);
    }
}
