
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct CardQuantityPair
{
    public Card Card;
    public int Quantity;
    public CardQuantityPair(Card card, int quantity)
    {
        Card = card;
        Quantity = quantity;
    }
}
[CreateAssetMenu(fileName = "NewCardPool")]
public class CardPool : ScriptableObject
{
    public List<CardQuantityPair> Cards;
}