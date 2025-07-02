using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] private Text betText, creditText;

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
}
