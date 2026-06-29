using System.Collections.Generic;
using UnityEngine;

public class FlagPlacementValidator : MonoBehaviour
{
    [SerializeField] private List<PlacementArea> _blockZones;
    [SerializeField] private PlacementArea _flagPlacementArea;

    public bool IsBlocked(Vector3 position)
    {
        for (int i = 0; i < _blockZones.Count; i++)
        {
           if (_blockZones[i].Inside(position))
                return true;
        }

        return false;
    }

    public void AddZone(PlacementArea flagInstallation) =>
        _blockZones.Add(flagInstallation);

    public bool CanPlace(Vector3 position)
    {
        if (_flagPlacementArea.Inside(position) == false || IsBlocked(position))
            return false;

        return true;
    }
}