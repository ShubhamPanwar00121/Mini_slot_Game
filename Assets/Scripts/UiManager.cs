using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] private ChangeValueUi setCoinPerLineUi, setPerCoinValueUi, setTotalBetUi;
    [SerializeField] private Transform setBetPanel, betMax;
    [SerializeField] private Text betText, creditText;
    public float CoinIncreaseTime = 1.5f;

    private float maxCoinPerLine = 10f;
    private float maxCoinValue = 0.5f;
    private float coinPerLine = 1f;
    private float coinValue = 0.1f;
    private float betAmount;
    private bool spinning = false;

    private void Start()
    {
        betAmount = coinPerLine * coinValue * 9;
        SetAmounts();
        setBetPanel.gameObject.SetActive(false);
        GameManager.Instance.SetUiManager(this);
        CustomEvents.creditChanged += GiveReward;
        CustomEvents.spinnComplete += SpinComplete;
    }

    private void OnDestroy()
    {
        CustomEvents.creditChanged -= GiveReward;
        CustomEvents.spinnComplete -= SpinComplete;
    }

    private void SpinComplete(CardBehaviour[,] matrix)
    {
        spinning = false;
    }

    private void SetAmounts()
    {
        betText.text = betAmount.ToString("F2");
        creditText.text = GameManager.Instance.GetBalance().ToString("F2");
    }

    public void Spinn()
    {
        spinning = true;
        GameManager.Instance.SetBalance(GameManager.Instance.GetBalance() - betAmount);
        SetAmounts();
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

    public void BetMax()
    {
        coinPerLine = maxCoinPerLine;
        coinValue = maxCoinValue;
        betAmount = coinPerLine * coinValue * 9;

        SetUi();
    }

    public void ChangeBetData(ChangeValueType cvt, bool increase)
    {
        if (spinning) return;

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

        coinPerLine = Mathf.Clamp(coinPerLine, 1f, maxCoinPerLine);
        coinValue = Mathf.Clamp(coinValue, 0.1f, maxCoinValue);

        betAmount = coinValue * coinPerLine * 9;
        SetUi();
    }

    private void SetUi()
    {
        setCoinPerLineUi.UpdateUi(coinPerLine, coinPerLine < maxCoinPerLine, coinPerLine > 1);
        setPerCoinValueUi.UpdateUi(coinValue, coinValue < maxCoinValue, coinValue > 0.1f);

        bool canIncreaseBet = (coinPerLine < maxCoinPerLine) || (coinValue < maxCoinValue);
        bool canDecreaseBet = (coinPerLine > 1) || (coinValue > 0.1f);

        betMax.gameObject.SetActive(canIncreaseBet);
        setTotalBetUi.UpdateUi(betAmount, canIncreaseBet, canDecreaseBet);
        SetAmounts();
    }

    public float GetBetAmount()
    {
        return betAmount;
    }

    public float GetPerCardBet()
    {
        return coinValue * coinPerLine;
    }

    public void GiveReward(double targetValue)
    {
        StartCoroutine(IncreaseCreditText(targetValue));
    }

    public IEnumerator IncreaseCreditText(double targetValue)
    {
        if (!double.TryParse(creditText.text, out double current))
        {
            current = 0.0;
        }

        float duration = CoinIncreaseTime;
        float elapsed = 0f;
        double startValue = current;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            double t = Mathf.Clamp01(elapsed / duration);
            double newValue = startValue + (targetValue - startValue) * t;

            creditText.text = newValue.ToString("F2");
            yield return null;
        }

        creditText.text = targetValue.ToString("F2");
    }
}
