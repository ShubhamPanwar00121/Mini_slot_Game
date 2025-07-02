using UnityEngine;
using UnityEngine.UI;

public class ChangeValueUi : MonoBehaviour
{
    [SerializeField] private ChangeValueType changeValueType;
    [SerializeField] private Button increaseBtn, DecreaseBtn;
    public UiManager uiManagerReff;

    private void Increase()
    {

    }

    private void Decrease() 
    {
    
    }
}

public enum ChangeValueType
{
    CoinPerLine,
    CoinValue,
    TotalBet,
}
