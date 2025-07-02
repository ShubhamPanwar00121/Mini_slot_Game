using System;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] private Text betText, creditText;
    [SerializeField] private Transform setBetPanel;
    private float maxCoinPerLine = 10f;
    private float MaxCoinValue = 1.0f;

    private void Start()
    {
        SetAmounts();
    }

    private void SetAmounts()
    {
        betText.text = GameManager.Instance.GetBet().ToString();
        creditText.text = GameManager.Instance.GetBalance().ToString();
    }

    public void Spinn()
    {
        CustomEvents.InvokeSpinn();
    }

    public void IncreaseBet()
    {
        if (!setBetPanel.gameObject.activeInHierarchy) setBetPanel.gameObject.SetActive(true);
    }

    public void DecreaseBet()
    {
        if (!setBetPanel.gameObject.activeInHierarchy) setBetPanel.gameObject.SetActive(true);
    }

    public void ChangeBetData(ChangeValueType cvt,bool increase, Action<bool,bool> callback = null)
    {

    }
}
