using System;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] private ChangeValueUi setCoinPerLineUi, setPerCoinValueUi, setTotalBetUi;
    [SerializeField] private Transform setBetPanel;
    [SerializeField] private Text betText, creditText;

    private float maxCoinPerLine = 10f;
    private float maxCoinValue = 0.5f;
    private float coinPerLine = 1f;
    private float coinValue = 0.1f;
    private float betAmount;

    private void Start()
    {
        betAmount = coinPerLine * coinValue;
        SetAmounts();
        setBetPanel.gameObject.SetActive(false);
        GameManager.Instance.SetUiManager(this);
    }

    private void SetAmounts()
    {
        betText.text = betAmount.ToString("F2");
        creditText.text = GameManager.Instance.GetBalance().ToString("F2");
    }

    public void Spinn()
    {
        CustomEvents.InvokeSpinn();
    }

    public void IncreaseBet()
    {
        if (!setBetPanel.gameObject.activeInHierarchy) setBetPanel.gameObject.SetActive(true);
        ChangeBetData(ChangeValueType.TotalBet, true);
    }

    public void DecreaseBet()
    {
        if (!setBetPanel.gameObject.activeInHierarchy) setBetPanel.gameObject.SetActive(true);
        ChangeBetData(ChangeValueType.TotalBet, false);
    }

    public void ChangeBetData(ChangeValueType cvt, bool increase)
    {
        switch (cvt)
        {
            case ChangeValueType.CoinPerLine:
                if (increase && coinPerLine < maxCoinPerLine)
                    coinPerLine++;
                else if (!increase && coinPerLine > 1)
                    coinPerLine--;
                break;

            case ChangeValueType.CoinValue:
                if (increase && coinValue < maxCoinValue)
                    coinValue += 0.1f;
                else if (!increase && coinValue > 0.1f)
                    coinValue -= 0.1f;
                break;

            case ChangeValueType.TotalBet:
                if (increase)
                {
                    if (coinPerLine < maxCoinPerLine)
                    {
                        coinPerLine++;
                    }
                    else if (coinValue < maxCoinValue)
                    { 
                        coinValue += 0.1f;
                        coinPerLine = 1;
                    }
                }
                else
                {
                    if (coinPerLine > 1)
                    {
                        coinPerLine--;
                    }
                    else if (coinValue > 0.1f)
                    { 
                        coinValue -= 0.1f; 
                        coinPerLine = maxCoinPerLine;
                    }
                }
                break;

            default:
                break;
        }

        // Clamp values just to be safe
        coinPerLine = Mathf.Clamp(coinPerLine, 1f, maxCoinPerLine);
        coinValue = Mathf.Clamp(coinValue, 0.1f, maxCoinValue);

        betAmount = coinValue * coinPerLine;
        SetUi();
    }

    private void SetUi()
    {
        setCoinPerLineUi.UpdateUi(coinPerLine, coinPerLine < maxCoinPerLine, coinPerLine > 1);
        setPerCoinValueUi.UpdateUi(coinValue, coinValue < maxCoinValue, coinValue > 0.1f);

        bool canIncreaseBet = (coinPerLine < maxCoinPerLine) || (coinValue < maxCoinValue);
        bool canDecreaseBet = (coinPerLine > 1) || (coinValue > 0.1f);

        setTotalBetUi.UpdateUi(betAmount, canIncreaseBet, canDecreaseBet);
        SetAmounts();
    }
}
