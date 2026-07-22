using UnityEngine;
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
    BlackNumber,
    PotOfGreed,
    RedHerring,
}