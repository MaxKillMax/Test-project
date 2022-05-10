using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private WinManager _winManager;

    private Health _playerHealth;

    public void SetWinManager(WinManager winManager) => _winManager = winManager;

    private void OnTriggerEnter(Collider other)
    {
        CheckForPlayer(other.transform);
    }

    private void CheckForPlayer(Transform transform)
    {
        if (transform.TryGetComponent<Player>(out Player player))
        {
            _playerHealth = player.health;
            _playerHealth.onPlayerDestroyed += OnPlayerDeath;
            _playerHealth.DestroyPlayer();
        }
    }

    private void OnPlayerDeath()
    {
        _playerHealth.onPlayerDestroyed -= OnPlayerDeath;
        _winManager.SetLose();
    }
}
