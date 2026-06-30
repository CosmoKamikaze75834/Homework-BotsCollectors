using UnityEngine;

public class ResourceAssignmentStrategyFactory : MonoBehaviour
{
    [SerializeField] private SelectionMode _selection;
    [SerializeField] private Counter _storage;

    public SelectionMode CurrentMode() => _selection;

    public IResourceSelectionStrategy CreateStrategy()
    {
        if (_selection == SelectionMode.Nearest)
            return new NearestStrategy();
        else if (_selection == SelectionMode.Deficit)
            return new DeficitStrategy(_storage);

        return null;
    }

    public void SetSelectionMode(SelectionMode selection)
    {
        _selection = selection;
    }
}