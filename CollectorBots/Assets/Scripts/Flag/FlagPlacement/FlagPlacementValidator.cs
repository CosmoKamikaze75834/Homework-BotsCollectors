using UnityEngine;

public class FlagPlacementValidator : MonoBehaviour
{
    [SerializeField] private PlacementArea _flagPlacementArea;

    public bool IsBlocked(Vector3 position)
    {
        BlockArea[] blockZones = FindObjectsByType<BlockArea>(FindObjectsSortMode.None);

        for (int i = 0; i < blockZones.Length; i++)
        {
            if (blockZones[i].IsInside(position))
                return true;
        }

        return false;
    }

    public bool CanPlace(Vector3 position) => 
        _flagPlacementArea.IsInside(position) && IsBlocked(position) == false;
}