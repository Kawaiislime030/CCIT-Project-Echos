using TMPro;
using UnityEngine;

public class HintUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hintText;

    public void ShowHint(string message, float duration)
    {
        gameObject.SetActive(true);
        hintText.text = message;
        CancelInvoke(nameof(ClearHint));
        Invoke(nameof(ClearHint), duration);
    }

    private void ClearHint()
    {
        hintText.text = "";
        gameObject.SetActive(false);
    }
}
