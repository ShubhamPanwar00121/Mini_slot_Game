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
        increaseBtn.gameObject.SetActive(enableIncrease);
        DecreaseBtn.gameObject.SetActive(enableDecrease);
    }
}

public enum ChangeValueType
{
    CoinPerLine,
    CoinValue,
    TotalBet,
}
