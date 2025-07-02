using System;
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
        CustomEvents.InvokeCreditChanged(balance + 10000);

        return;
        float totalReward = 0f;
        int rowsToCheck = 3;
        int columnCount = finalMatrix.GetLength(0);
        CardType matchedType = CardType.None;

        for (int y = 0; y < rowsToCheck; y++)
        {
            CardType baseType = CardType.None;
            int matchCount = 0;

            for (int x = 0; x < columnCount; x++)
            {
                CardBehaviour card = finalMatrix[x, y];

                if (x == 0)
                {
                    if (card.CardType == CardType.Wild)
                    {
                        baseType = CardType.Wild;
                        matchCount += 1;
                        matchedType = card.CardType;
                    }
                    else if (card.CardType != CardType.Scatter)
                    {
                        baseType = card.CardType;
                        matchCount += 1;
                        matchedType = card.CardType;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    if (baseType == CardType.Wild && card.CardType != CardType.Scatter)
                    {
                        matchCount += 1;
                        baseType = card.CardType;
                        matchedType = card.CardType;
                    }
                    else if (card.CardType == baseType || card.CardType == CardType.Wild)
                    {
                        matchCount += 1;
                        matchedType = card.CardType;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            if (matchCount >= 3 && matchedType != CardType.Wild && matchedType != CardType.Scatter)
            { 
                totalReward += matchCount * GetCardMultiFact(baseType);
                print(y);
            }

            print(matchedType);
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
            Debug.Log($"Scatter win! Count: {scatterCount}");
        }

        if (totalReward > 0f)
        {
            float reward = totalReward * uiManager.GetPerCardBet();
            CustomEvents.InvokeCreditChanged(balance + reward);
        }
        else
        {
            Debug.Log("No win.");
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
    public static event Action<double> creditChanged;
    public static event Action spinn;
    public static event Action<CardBehaviour[,]> spinnComplete;

    public static void InvokeBetChanged(double val)
    {
        betChanged?.Invoke(val);
    }

    public static void InvokeCreditChanged(double val)
    {
        creditChanged?.Invoke(val);
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
