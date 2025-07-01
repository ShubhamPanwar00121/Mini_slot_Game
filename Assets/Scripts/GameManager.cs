using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private SlotDataScriptable slotData;

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
}
