using UnityEngine;

public class Finish : MonoBehaviour
{
    private WinManager _winManager;

    private Player _player;

    public void SetWinManager(WinManager winManager) => _winManager = winManager;

    private void OnTriggerEnter(Collider other)
    {
        CheckForPlayer(other.transform);
    }

    private void CheckForPlayer(Transform transform)
    {
        if (transform.TryGetComponent<Player>(out Player player))
        {
            _player = player;
            _player.onPlayerWin += OnPlayerWin;
            _player.Win();
        }
    }

    private void OnPlayerWin()
    {
        _player.onPlayerWin -= OnPlayerWin;
        _winManager.SetWin();
    }
}
