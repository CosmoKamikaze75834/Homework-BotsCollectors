using TMPro;
using UnityEngine;

public class TextChanger : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Counter _counter;
    [SerializeField] private ResourceType _type;

    private void OnEnable() => _counter.Changed += UpdateUI;

    private void OnDisable() => _counter.Changed -= UpdateUI;

    private void UpdateUI(ResourceType type, int newAmount)
    {
        if (type == _type)
            _text.text = $"{_type}: {newAmount}";
    }
}