using System.Collections.Generic;
using UnityEngine;
using System;

public sealed class MapGenerator : MonoBehaviour
{
    [SerializeField] private WinManager _winManager;

    [SerializeField] private Transform _cellParent;
    [SerializeField] private Cell _cellPrefab;
    [SerializeField] private Finish _finishPrefab;
    [SerializeField] private DeadZone _deadZonePrefab;

    [SerializeField] private bool _createDeadZones = true;

    private Cell _startCell;
    private Cell _endCell;

    private Cell _createdCell;
    private List<Cell> _connectedNeighbors;

    private (int, int) _startPosition;
    private (int, int) _endPosition;

    private float _scaleOfCell = 1.5f;
    private int _width = 15;
    private int _length = 15;

    private int _currentGeneration = 0;
    private float _chanceToCreateDeadZone = 0.15f;
    private float _chanceIncreaseDeadZone = 0.05f;

    private bool _recreate = false;

    private List<Cell> _connectedToStart;
    private Cell[,] _map;

    public Cell startCell => _startCell;
    public Cell endCell => _endCell;

    public event Action onMapGenerated;

    private void Awake()
    {
        _startPosition = (0, _width - 1);
        _endPosition = (_length - 1, 0);

        _winManager.onPlayerWin += NextMap;
        _winManager.onPlayerLose += ResetMap;
    }

    private void Start()
    {
        GenerateMap(true);   
    }

    private void OnDestroy()
    {
        _winManager.onPlayerWin -= NextMap;
        _winManager.onPlayerLose -= ResetMap;
    }

    [ContextMenu("Next map")]
    public void NextMap() => GenerateMap(true);

    [ContextMenu("Reset map")]
    public void ResetMap() => GenerateMap(false);

    private void GenerateMap(bool additionaly = true)
    {
        if (_map != null)
            DestroyMap();

        CheckMapSize();

        _map = new Cell[_length, _width];
        _connectedToStart = new List<Cell>();

        FillMap();
        InitializePoints();
        CreatePassages();
        if (_createDeadZones)
            CreateDeadZones();

        if (_recreate)
        {
            Debug.LogWarning("Error recreate");

            _recreate = false;
            GenerateMap();
            return;
        }

        if (additionaly)
            _currentGeneration++;

        onMapGenerated?.Invoke();
    }

    private void FillMap()
    {
        for (int x = 0; x < _map.GetLength(0); x++)
        {
            for (int y = 0; y < _map.GetLength(1); y++)
            {
                CreateCell(new Vector3Int(x, 0, y));
                _map[x, y] = _createdCell;
            }
        }
    }

    private void InitializePoints()
    {
        _startCell = _map[_startPosition.Item1, _startPosition.Item2];
        _endCell = _map[_endPosition.Item1, _endPosition.Item2];

        _connectedToStart.Add(_startCell);
        Finish finish = Instantiate<Finish>(_finishPrefab, _endCell.GetFinishPosition(), Quaternion.identity);
        finish.transform.SetParent(_endCell.transform);
        finish.SetWinManager(_winManager);
    }

    private void CreatePassages()
    {
        int maxCount = _map.GetLength(0) * _map.GetLength(1);
        int currentCount = 1;
        int current—ycle = 1;
        int maxCycle = (int)(maxCount * 1.2f);

        bool start = true;
        bool ended = false;

        while (!ended)
        {
            current—ycle++;

            for (int i = _connectedToStart.Count - 1; i >= 0; i--)
            {
                if (ConnectToFreeNeighbors(_connectedToStart[i]))
                {
                    for (int o = 0; o < _connectedNeighbors.Count; o++)
                    {
                        _connectedToStart.Add(_connectedNeighbors[o]);
                        currentCount++;
                    }
                }

                if (!start || !(MapNotEnded(currentCount, maxCount) && OneConnected() && _connectedNeighbors.Count == 0))
                    _connectedToStart.RemoveAt(i);
            }

            if (_connectedToStart.Count > 3 || _connectedToStart.Count >= (_map.GetLength(0) + _map.GetLength(1)) / 2)
                start = false;

            if (_connectedToStart.Count == 0 || IsErrorPassageCycle(current—ycle, maxCycle))
            {
                ended = true;

                if (IsErrorPassageCycle(current—ycle, maxCycle) || IsErrorPassageCount(currentCount, maxCount))
                    _recreate = true;
            }
        }
    }

    private void CreateDeadZones()
    {
        float sumChance = _chanceToCreateDeadZone + (_chanceIncreaseDeadZone * _currentGeneration);

        for (int x = 0; x < _map.GetLength(0); x++)
        {
            for (int y = 0; y < _map.GetLength(1); y++)
            {
                if (!PositionIsNearToBounds(x, y) && !IsEndOrStart(_map[x, y]) && GotChance(sumChance))
                {
                    if (!_map[x, y].CheckBottomWall())
                        CreateDeadZone(_map[x, y], false);
                    else if (!_map[x, y].CheckLeftWall())
                        CreateDeadZone(_map[x, y], true);
                }
            }
        }
    }

    private void CheckMapSize()
    {
        if (_width < 2)
            _width = 2;
        if (_length < 2)
            _length = 2;
    }

    private void DestroyMap()
    {
        for (int i = 0; i < _map.GetLength(0); i++)
        {
            for (int o = 0; o < _map.GetLength(1); o++)
            {
                Destroy(_map[i, o].gameObject);
            }
        }
    }

    private void CreateCell(Vector3Int position)
    {
        _createdCell = Instantiate(_cellPrefab);
        _createdCell.Initialize(position.x, position.z);
        _createdCell.transform.SetParent(_cellParent);
        _createdCell.transform.position = (Vector3)position * _scaleOfCell;
    }

    private void CreateDeadZone(Cell cell, bool onLeft)
    {
        DeadZone deadZone = Instantiate(_deadZonePrefab);
        deadZone.transform.SetParent(cell.transform);
        deadZone.transform.position = onLeft ? cell.GetLeftDeadZonePosition() : cell.GetBottomDeadZonePosition();
        deadZone.SetWinManager(_winManager);
    }

    private bool ConnectToFreeNeighbors(Cell cell)
    {
        _connectedNeighbors = new List<Cell>();

        if (FindNeighbor(cell.x + 1, cell.y))
            _connectedNeighbors[_connectedNeighbors.Count - 1].ToggleXWall();
        if (FindNeighbor(cell.x - 1, cell.y))
            cell.ToggleXWall();
        if (FindNeighbor(cell.x, cell.y + 1))
            _connectedNeighbors[_connectedNeighbors.Count - 1].ToggleZWall();
        if (FindNeighbor(cell.x, cell.y - 1))
            cell.ToggleZWall();

        if (_connectedNeighbors.Count != 0)
            return true;

        return false;
    }

    private bool FindNeighbor(int x, int y)
    {
        if (!PositionIsOutOfBounds(x, y) && !_map[x, y].OutOfLimit() && _map[x, y].IsReadyToConnect())
        {
            _connectedNeighbors.Add(_map[x, y]);
            _map[x, y].AddConnection();
            return true;
        }

        return false;
    }

    private bool IsErrorPassageCount(int count, int maxCount) => count < maxCount;

    private bool IsErrorPassageCycle(int cycle, int maxCycle) => cycle > maxCycle;

    private bool IsEndOrStart(Cell cell) => cell.Equals(_startCell) || cell.Equals(_endCell);

    private bool GotChance(float chance) => UnityEngine.Random.Range(0f, 1f) < chance; 

    private bool OneConnected() => _connectedToStart.Count == 1;

    private bool MapNotEnded(int currentCount, int maxCount) => currentCount < maxCount;

    private bool PositionIsOutOfBounds(int x, int y) => x >= _length || x < 0 || y >= _width || y < 0;

    private bool PositionIsNearToBounds(int x, int y) => x >= _length - 1 || x <= 0 || y >= _width - 1 || y <= 0;
}
