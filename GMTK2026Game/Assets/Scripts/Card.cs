using System;
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
[CreateAssetMenu(fileName = "NewCard")]
public class Card : ScriptableObject
{
    public CardType CardType;
    public int Number;
    public JokerType JokerType;
}
public enum CardType
{
    Number,
    Joker,
    Special,
    Event,
}
public enum JokerType
{
    None,
    BlackNumber,
    PotOfGreed,
    RedHerring,
}