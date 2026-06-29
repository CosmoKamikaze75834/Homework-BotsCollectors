using System;
using UnityEngine;

public class RaycastHandler : MonoBehaviour
{
    private const string LayerNameBlock = "FlagPlacementBlocker";

    [SerializeField] private MouseInputHandler _mouseInput;

    public event Action <RaycastHit> OnRaycastHit;
    public event Action <RaycastHit> OnMouseMoveHit;

    private void OnEnable()
    {
        _mouseInput.Clicked += ProcessClickRaycast;
        _mouseInput.MouseMoved += ProcessMoveRaycast;
    }

    private void OnDisable()
    {
        _mouseInput.Clicked -= ProcessClickRaycast;
        _mouseInput.MouseMoved -= ProcessMoveRaycast;
    }

    private void ProcessClickRaycast(Vector2 mousePosition) =>
        ProcessRaycast(mousePosition, OnRaycastHit);

    private void ProcessMoveRaycast(Vector2 mousePosition) =>
        ProcessRaycast(mousePosition, OnMouseMoveHit);

    private RaycastHit? TryRaycast(Vector2 mousePosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        int layerMask = ~LayerMask.GetMask(LayerNameBlock);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
            return hit;

        return null;
    }

    private void ProcessRaycast(Vector2 mousePosition, Action<RaycastHit> callback)
    {
        RaycastHit? hit = TryRaycast(mousePosition);

        if (hit != null)
            callback?.Invoke(hit.Value);
    }
}