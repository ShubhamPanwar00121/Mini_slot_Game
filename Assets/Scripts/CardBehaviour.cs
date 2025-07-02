using UnityEngine;
using UnityEngine.UI;

public class CardBehaviour : MonoBehaviour
{
    public RectTransform rectTransform;
    [SerializeField] private Image Icon;
    public float MultiFact;
    public CardType CardType;
    public int Index;

    public void SetCard(Sprite icon, CardType type, float multiFact, int index)
    {
        Icon.sprite = icon;
        CardType = type;
        MultiFact = multiFact;
        Index = index;
    }
}
