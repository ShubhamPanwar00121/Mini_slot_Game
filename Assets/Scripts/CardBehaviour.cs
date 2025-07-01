using UnityEngine;
using UnityEngine.UI;

public class CardBehaviour : MonoBehaviour
{
    public RectTransform rectTransform;
    [SerializeField] private Image Icon;
    private float multiFact;
    private CardType cardType;
    private int index;

    public void SetCard(Sprite icon, CardType type, float multiFact, int index)
    {
        Icon.sprite = icon;
        cardType = type;
        this.multiFact = multiFact;
        this.index = index;
    }
}
