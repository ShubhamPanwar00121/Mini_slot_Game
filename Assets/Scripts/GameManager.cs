using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private SlotDataScriptable slotData;
    public static GameManager Instance;
    private double balance
    {
        get => double.Parse(PlayerPrefs.GetString("BALANCE", "100000"));
        set => PlayerPrefs.SetString("BALANCE", value.ToString());
    }
    private UiManager uiManager;
    private List<ShakeAnimation> shakeAnimations = new List<ShakeAnimation>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            CustomEvents.spinnComplete += DecideReward;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        CustomEvents.spinnComplete -= DecideReward;
    }

    public SlotCard GetRandomCard()
    {
        int i = UnityEngine.Random.Range(0, slotData.slotCards.Count);
        return slotData.slotCards[i];
    }

    public double GetBalance() 
    {
        return balance;
    }

    public void SetBalance(double balance)
    {
        this.balance = balance;
    }

    public UiManager GetUiManager()
    {
        return uiManager;
    }

    public void SetUiManager(UiManager um)
    {
        uiManager = um;
    }

    private void DecideReward(CardBehaviour[,] finalMatrix)
    {
        float totalReward = 0f;
        int rowsToCheck = 3;
        int columnCount = finalMatrix.GetLength(0);
        CardType matchedType = CardType.None;
        shakeAnimations.Clear();

        for (int y = 0; y < rowsToCheck; y++)
        {
            List<ShakeAnimation> temp = new List<ShakeAnimation>();
            CardType baseType = CardType.None;
            int matchCount = 0;

            for (int x = 0; x < columnCount; x++)
            {
                CardBehaviour card = finalMatrix[x, y];

                if (x == 0)
                {
                    if (card.CardType != CardType.Scatter)
                    {
                        baseType = card.CardType;
                        matchCount += 1;
                        matchedType = card.CardType;
                        temp.Add(card.GetComponent<ShakeAnimation>());
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    if(card.CardType == CardType.Scatter)
                    {
                        break;
                    }
                    else if (baseType == CardType.Wild)
                    {
                        matchCount += 1;
                        baseType = card.CardType;
                        matchedType = card.CardType;
                        temp.Add(card.GetComponent<ShakeAnimation>());
                    }
                    else if (card.CardType == baseType || card.CardType == CardType.Wild)
                    {
                        matchCount += 1;
                        temp.Add(card.GetComponent<ShakeAnimation>());
                    }
                    else
                    {
                        break;
                    }
                }
            }

            if (matchCount >= 3 && matchedType != CardType.Wild && matchedType != CardType.None)
            { 
                totalReward += matchCount * GetCardMultiFact(baseType);
                foreach (ShakeAnimation sa in temp)
                {
                    shakeAnimations.Add(sa);
                }
            }
        }

        int scatterCount = 0;

        foreach (CardBehaviour card in finalMatrix)
        {
            if (card.CardType == CardType.Scatter)
            {
                scatterCount++;
            }
        }

        if (scatterCount >= 3)
        {
            uiManager.GiveFreeSpin();
        }
        else
        {
            uiManager.NoFreeSpin();
        }

        if (totalReward > 0f)
        {
            float reward = totalReward * uiManager.GetPerCardBet();
            CustomEvents.InvokeCreditChanged(balance + reward, reward);

            foreach (ShakeAnimation sa in shakeAnimations)
            {
                sa.enabled = true;
            }
        }
    }

    private float GetCardMultiFact(CardType ct)
    {
        foreach (SlotCard sc in slotData.slotCards)
        {
            if (sc.cardType == ct)
                return sc.multiFactor;
        }

        return 0f;
    }

}

public static class CustomEvents
{
    public static event Action<double> betChanged;
    public static event Action<double, float> creditChanged;
    public static event Action spinn;
    public static event Action<CardBehaviour[,]> spinnComplete;

    public static void InvokeBetChanged(double val)
    {
        betChanged?.Invoke(val);
    }

    public static void InvokeCreditChanged(double val1, float val2)
    {
        creditChanged?.Invoke(val1, val2);
    }

    public static void InvokeSpinn()
    {
        spinn?.Invoke();
    }

    public static void InvokeSpinnComplete(CardBehaviour[,] finalMat)
    {
        spinnComplete?.Invoke(finalMat);
    }
}
