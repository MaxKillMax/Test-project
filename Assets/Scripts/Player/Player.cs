using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    public event Action onPlayerWin;

    [SerializeField] private Movement _movement;
    public Movement movement => _movement;

    [SerializeField] private Health _health;
    public Health health => _health;

    public void Win()
    {
        _health.onPlayerDestroyed += EndWin;
        _health.DestroyPlayer();
    }

    private void EndWin()
    {
        _health.onPlayerDestroyed -= EndWin;
        onPlayerWin?.Invoke();
    }
}
