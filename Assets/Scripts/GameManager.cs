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
        }
        else
        {
            Destroy(gameObject);
        }
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
}

public static class CustomEvents
{
    public static event Action<double> betChanged;
    public static event Action<double> creditChanged;
    public static event Action spinn;

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
}
