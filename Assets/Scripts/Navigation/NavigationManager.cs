using UnityEngine;
using System.Collections;
using Unity.AI.Navigation;

public class NavigationManager : MonoBehaviour
{
    [SerializeField] private TimeManager _timeManager;
    [SerializeField] private MapGenerator _mapGenerator;
    [SerializeField] private PlayerManager _playerGenerator;
    [SerializeField] private NavMeshSurface _navMeshSurface;

    private float _waitTime = 1f;
    
    private void Awake()
    {
        _timeManager.onGameUnpaused += SetDestination;
        _mapGenerator.onMapGenerated += RebuildNavMesh;
        _mapGenerator.onMapGenerated += SetDestination;
    }

    private void OnDestroy()
    {
        _timeManager.onGameUnpaused -= SetDestination;
        _mapGenerator.onMapGenerated -= RebuildNavMesh;
        _mapGenerator.onMapGenerated -= SetDestination;
    }

    [ContextMenu("Debug restart")]
    private void DebugRestart()
    {
        RebuildNavMesh();
        SetDestination();
    }

    private void RebuildNavMesh()
    {
        _navMeshSurface.BuildNavMesh();
        StartCoroutine(WaitForSecondRebuild());
    }

    private IEnumerator WaitForSecondRebuild()
    {
        yield return new WaitForSeconds(_waitTime);
        _navMeshSurface.BuildNavMesh();
    }

    public void SetDestination()
    {
        if (!_timeManager.onPause)
            _playerGenerator.player.movement.SetDestination(_mapGenerator.endCell.GetPosition());
    }
}
