using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SlotData", menuName = "SlotGame/SlotData")]
public class SlotDataScriptable : ScriptableObject
{
    public List<SlotCard> slotCards = new List<SlotCard>();
}

[System.Serializable]
public class SlotCard
{
    public int CardIndex;
    public Sprite Icon;
    public float multiFactor;
    public CardType cardType;
}

public enum CardType
{
    Normal,
    Wild,
    Scatter,
    Q,
    J,
    K
}