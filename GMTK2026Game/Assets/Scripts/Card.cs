using System;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewCard")]
public class Card : ScriptableObject
{
    public CardType CardType;
    public int Number;
    public Card NextNumber;
    public Card PreviousNumber;
    public JokerType JokerType;
    public SpecialType SpecialType;
    public EventType EventType;
    public string Description;
    public Sprite Sprite;
    public Sprite BackSideSprite;
    public override string ToString()
    {
        return $"[{CardType},{Number},{JokerType},{SpecialType}]";
    }
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
    Taxes,
}
public enum SpecialType
{
    None,
    Add,
    Subtract,
    Steal,
    Trade,
    Skip,
    Reverse,

}
public enum EventType
{
    None,
    Friday13th,
    Inflation,
    RobinHood,
    Christmas,
    Communism,
    // DeusExMachina,
}