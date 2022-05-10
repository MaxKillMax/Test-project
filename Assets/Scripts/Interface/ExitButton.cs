using UnityEngine;

public class ExitButton : MonoBehaviour
{
    [SerializeField] private InterfaceManager _interfaceManager;

    public void OnExitSetted()
    {
        _interfaceManager.QuitGame();
    }
}
