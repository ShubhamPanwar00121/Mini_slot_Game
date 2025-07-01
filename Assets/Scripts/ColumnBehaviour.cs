using System.Collections.Generic;
using UnityEngine;

public class ColumnBehaviour : MonoBehaviour
{
    [SerializeField] private RectTransform parentRect;
    [SerializeField] private CardBehaviour cardBehaviour;
    private List<CardBehaviour> activeCardItems = new List<CardBehaviour>();
    private float cardHeight, spacing = 30f, lastScrollPosSnap = 0, topSpacing = 6f;
    private int topIndex, bottomIndex;

    private void Start()
    {
        SpawnInitial();
    }

    private void SpawnInitial()
    {
        CardBehaviour p = Instantiate(cardBehaviour, parentRect);
        activeCardItems.Add(p);
        cardHeight = p.rectTransform.rect.height;

        int toSpawn = Mathf.CeilToInt(Screen.height / (cardHeight + spacing));

        for (int i = 0; i < toSpawn; i++)
        {
            p = Instantiate(cardBehaviour, parentRect);
            activeCardItems.Add(p);
        }

        InitialItemsPositionSetInScrollView();
    }

    private void InitialItemsPositionSetInScrollView()
    {
        RectTransform rect;

        for (int i = 0; i < activeCardItems.Count; i++)
        {
            rect = activeCardItems[i].rectTransform;
            rect.anchoredPosition = new Vector2(0, -(topSpacing + i * (cardHeight + spacing) + cardHeight / 2f));

            SlotCard sc = GameManager.Instance.GetRandomCard();
            activeCardItems[i].SetCard(sc.Icon, sc.cardType, sc.multiFactor, sc.CardIndex);
        }

        parentRect.anchoredPosition = Vector2.right * parentRect.anchoredPosition.x;
        topIndex = 0;
        bottomIndex = activeCardItems.Count - 1;
    }

    private void Update()
    {
        if (parentRect.anchoredPosition.y > (lastScrollPosSnap + cardHeight + spacing / 2f) + topSpacing)
        {
            lastScrollPosSnap += (cardHeight + spacing);
            AddNewAtLast();
        }

        else if (parentRect.anchoredPosition.y < lastScrollPosSnap)
        {
            lastScrollPosSnap -= (cardHeight + spacing);
            AddNewAtFirst();
        }
    }

    private void AddNewAtLast()
    {
        RectTransform lastRect = (RectTransform)parentRect.transform.GetChild(parentRect.transform.childCount - 1);
        RectTransform firstRect = (RectTransform)parentRect.transform.GetChild(0);

        firstRect.anchoredPosition = new Vector2(0, lastRect.anchoredPosition.y - (cardHeight + spacing));
        firstRect.SetAsLastSibling();

        topIndex++;
        bottomIndex++;

        SlotCard sc = GameManager.Instance.GetRandomCard();
        firstRect.GetComponent<CardBehaviour>().SetCard(sc.Icon, sc.cardType, sc.multiFactor, sc.CardIndex);
    }

    private void AddNewAtFirst()
    {
        RectTransform lastRect = (RectTransform)parentRect.transform.GetChild(parentRect.transform.childCount - 1);
        RectTransform firstRect = (RectTransform)parentRect.transform.GetChild(0);

        lastRect.anchoredPosition = new Vector2(0, firstRect.anchoredPosition.y + (cardHeight + spacing));
        lastRect.SetAsFirstSibling();

        topIndex--;
        bottomIndex--;

        SlotCard sc = GameManager.Instance.GetRandomCard();
        lastRect.GetComponent<CardBehaviour>().SetCard(sc.Icon, sc.cardType, sc.multiFactor, sc.CardIndex);
    }
}
