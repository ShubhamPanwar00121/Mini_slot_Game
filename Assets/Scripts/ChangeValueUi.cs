using UnityEngine;
using UnityEngine.UI;

public class ChangeValueUi : MonoBehaviour
{
    [SerializeField] private ChangeValueType changeValueType;
    [SerializeField] private Button increaseBtn, DecreaseBtn;
    [SerializeField] private Text valueText;

    private void OnEnable()
    {
        increaseBtn.onClick.AddListener(Increase);
        DecreaseBtn.onClick.AddListener(Decrease);
    }

    private void OnDisable()
    {
        increaseBtn.onClick.RemoveAllListeners();
        DecreaseBtn.onClick.RemoveAllListeners();
    }

    private void Increase()
    {
        GameManager.Instance.GetUiManager().ChangeBetData(changeValueType, true);
    }

    private void Decrease() 
    {
        GameManager.Instance.GetUiManager().ChangeBetData(changeValueType, false);
    }

    public void UpdateUi(float val, bool enableIncrease, bool enableDecrease)
    {
        valueText.text = val.ToString();
        increaseBtn.enabled = enableIncrease;
        DecreaseBtn.enabled = enableDecrease;
    }
}

public enum ChangeValueType
{
    CoinPerLine,
    CoinValue,
    TotalBet,
}
