using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private TimeManager _timeManager;
    [SerializeField] private MapGenerator _mapGenerator;

    [SerializeField] private Transform _playerParent;
    [SerializeField] private Player _playerPrefab;

    private Player _player;

    public Player player => _player;

    private void Awake()
    {
        _mapGenerator.onMapGenerated += CreatePlayer;
    }

    private void OnDestroy()
    {
        _mapGenerator.onMapGenerated -= CreatePlayer;
    }

    private void CreatePlayer()
    {
        if (_player != null)
            DestroyOldPlayer();

        _player = Instantiate<Player>(_playerPrefab, _mapGenerator.startCell.GetPosition(), Quaternion.identity);
        _timeManager.onGamePaused += _player.movement.Stop;
        _player.transform.SetParent(_playerParent);
    }

    private void DestroyOldPlayer()
    {
        _timeManager.onGamePaused -= _player.movement.Stop;
        Destroy(_player.gameObject);
    }
}
