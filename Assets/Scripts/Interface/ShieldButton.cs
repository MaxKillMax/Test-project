using UnityEngine;
using UnityEngine.EventSystems;

public class ShieldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private PlayerManager _playerGenerator;

    public void OnPointerDown(PointerEventData eventData)
    {
        _playerGenerator.player.health.StartImmortality();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _playerGenerator.player.health.EndImmortality();
    }
}
