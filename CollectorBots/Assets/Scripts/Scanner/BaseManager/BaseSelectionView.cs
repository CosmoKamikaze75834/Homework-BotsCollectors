using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class BaseSelectionView : MonoBehaviour
{
    [SerializeField] private Material _defaultMaterial;
    [SerializeField] private Material _selectedMaterial;

    private Renderer _renderer;

    private void Start() => 
        _renderer = GetComponent<Renderer>();

    public void Select() => 
        _renderer.material = _selectedMaterial;

    public void Deselect() => 
        _renderer.material = _defaultMaterial;
}