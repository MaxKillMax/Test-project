using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private Transform _position;
    [SerializeField] private Transform _finishPosition;

    [SerializeField] private Transform _bottomDeadZonePosition;
    [SerializeField] private Transform _leftDeadZonePosition;

    [SerializeField] private GameObject _bottomWall;
    [SerializeField] private GameObject _leftWall;

    private bool _isReadyToConnect;
    private int _connectionsLimit;
    private int _currentConnections = 0;

    private int _xPosition;
    private int _yPosition;

    public int x => _xPosition;
    public int y => _yPosition;

    public void Initialize(int x, int y)
    {
        _isReadyToConnect = Random.Range(0, 2) == 1 ? true : false;
        _connectionsLimit = Random.Range(1, 2);

        _xPosition = x;
        _yPosition = y;
    }

    public bool IsReadyToConnect()
    {
        if (_isReadyToConnect)
            return true;

        _isReadyToConnect = true;
        return false;
    }

    public Vector3 GetPosition() => _position.position;

    public bool OutOfLimit() => _currentConnections >= _connectionsLimit;

    public void AddConnection() => _currentConnections++;

    public void ToggleZWall() => _bottomWall.SetActive(false);

    public void ToggleXWall() => _leftWall.SetActive(false);

    public bool CheckBottomWall() => _bottomWall.activeSelf;

    public bool CheckLeftWall() => _leftWall.activeSelf;

    public Vector3 GetFinishPosition() => _finishPosition.position;

    public Vector3 GetBottomDeadZonePosition() => _bottomDeadZonePosition.position;

    public Vector3 GetLeftDeadZonePosition() => _leftDeadZonePosition.position;
}
