
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewCardPool")]
public class CardPool : ScriptableObject
{
    public List<CardQuantityPair> Cards;
}