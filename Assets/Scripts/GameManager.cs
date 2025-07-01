using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private SlotDataScriptable slotData;
    public static GameManager Instance;
    private double betAmount;
    private double balance
    {
        get => double.Parse(PlayerPrefs.GetString("BALANCE", "100000"));
        set => PlayerPrefs.SetString("BALANCE", value.ToString());
    }

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
        int i = Random.Range(0, slotData.slotCards.Count);
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

    public double GetBet()
    {
        return betAmount;
    }

    public void SetBet(double betAmount)
    {
        this.betAmount = betAmount;
    }
}
