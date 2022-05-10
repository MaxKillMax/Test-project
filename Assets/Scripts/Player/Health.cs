using UnityEngine;
using System;
using System.Collections;

public class Health : MonoBehaviour
{
    public event Action onPlayerDestroyed;

    [SerializeField] private Skin _skin;
    [SerializeField] private Movement _movement;
    [SerializeField] private ParticleSystem _particleSystem;

    private float _waitTime = 0.6f;

    private bool _immortality = false;
    private float _immortalityTime = 2;

    private bool _isDeath = false;

    public bool isDeath => _isDeath;

    public void DestroyPlayer()
    {
        if (_immortality)
            return;

        _movement.Stop();
        _particleSystem.Play();
        _isDeath = true;
        StartCoroutine(WaitForDestroy(_waitTime));
    }

    private IEnumerator WaitForDestroy(float time)
    {
        yield return new WaitForSeconds(time);

        onPlayerDestroyed?.Invoke();
        Destroy(gameObject);
    }

    private IEnumerator WaitForEndOfImmortality()
    {
        yield return new WaitForSeconds(_immortalityTime);

        EndImmortality();
    }

    public void StartImmortality()
    {
        if (_isDeath)
            return;

        _immortality = true;
        _skin.SetImmortalityMaterial();
        StopAllCoroutines();
        StartCoroutine(WaitForEndOfImmortality());
    }

    public void EndImmortality()
    {
        _immortality = false;
        _skin.SetCommonMaterial();
    }
}
