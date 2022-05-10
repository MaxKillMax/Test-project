using UnityEngine;
using System;

public class WinManager : MonoBehaviour
{
    public event Action onPlayerWin;
    public event Action onPlayerLose;

    public void SetWin()
    {
        onPlayerWin?.Invoke();
    }

    public void SetLose()
    {
        onPlayerLose?.Invoke();
    }
}
