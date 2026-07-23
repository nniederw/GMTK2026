using System;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewCard")]
public class Card : ScriptableObject
{
    public CardType CardType;
    public int Number;
    public JokerType JokerType;
    public Sprite Sprite;
    public Sprite BackSideSprite;
    public override string ToString()
    {
        return $"[{CardType},{Number},{JokerType}]";
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
}