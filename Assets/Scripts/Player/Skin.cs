using UnityEngine;

public class Skin : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;

    [SerializeField] private Material _immortalityMaterial;
    [SerializeField] private Material _commonMaterial;

    public void SetImmortalityMaterial()
    {
        _meshRenderer.material = _immortalityMaterial;
    }

    public void SetCommonMaterial()
    {
        _meshRenderer.material = _commonMaterial;
    }
}
