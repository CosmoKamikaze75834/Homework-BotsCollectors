using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceStrategyView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _strategyText;
    [SerializeField] private BaseConstructionSender _sender;

    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _previousButton;

    private void Start() =>
        UpdateText();

    private void OnEnable()
    {
        _nextButton.onClick.AddListener(NextStrategy);
        _previousButton.onClick.AddListener(PreviousStrategy);
    }

    private void OnDisable()
    {
        _nextButton.onClick.RemoveListener(NextStrategy);
        _previousButton.onClick.RemoveListener(PreviousStrategy);
    }

    public void NextStrategy()
    {
        SelectionMode current = _sender.GetCurrentStrategy();

        SelectionMode next;

        if (current == SelectionMode.Nearest)
            next = SelectionMode.Deficit;
        else
            next = SelectionMode.Nearest;

        _sender.ChangeStartegy(next);

        UpdateText();
    }

    private void PreviousStrategy() =>
        NextStrategy();


    private void UpdateText() =>
        _strategyText.text = _sender.GetCurrentStrategy().ToString();
}