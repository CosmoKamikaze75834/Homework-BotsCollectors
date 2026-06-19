using System;
using UnityEngine;

public class RaycastHandler : MonoBehaviour
{
    [SerializeField] private MouseInputHandler _mouseInput;

    public event Action <RaycastHit> OnRaycastHit;
    public event Action<RaycastHit> OnMouseMoveHit;

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

    private void ProcessClickRaycast(Vector2 mousePosition)
    {
        RaycastHit? hit = LaunchRaycast(mousePosition);
        if (hit.HasValue)
        {
            OnRaycastHit?.Invoke(hit.Value);
        }
    }

    private void ProcessMoveRaycast(Vector2 mousePosition)
    {
        RaycastHit? hit = LaunchRaycast(mousePosition);
        if (hit.HasValue)
        {
            OnMouseMoveHit?.Invoke(hit.Value);
        }
    }

    private RaycastHit? LaunchRaycast(Vector2 mousePosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 1f);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log(hit.collider.name);
            Debug.Log(hit.collider.transform.position);
            return hit;
        }

        return null;  
    }
}