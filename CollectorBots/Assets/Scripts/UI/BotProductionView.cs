using TMPro;
using UnityEngine;

public class BotProductionView : MonoBehaviour
{
    [SerializeField] private TextMeshPro _botText;

    private void Awake() =>
        Hide();

    public void Show() =>
        _botText.gameObject.SetActive(true);

    public void Hide() =>
        _botText.gameObject.SetActive(false);
}