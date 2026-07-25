using System;
using System.Collections.Generic;
using UnityEngine;
public class Player
{
    private List<Card> Cards = new();
    public CardGame CardGame = null;
    public Player()
    {
        Cards = new();
        CardGame = null; //should be assigned by the creation of the CardGame;
    }
    public void AddCard(Card card)
    {
        Cards.Add(card);
    }
    public void PlayCard(Card card)
    {
        int index = Cards.IndexOf(card);
        PlayCard(index);
    }
    public void PlayCard(int index)
    {
        if (index < 0 || index >= Cards.Count)
        {
            throw new System.Exception("Tried playing an invalid index of a card!");
        }
        Card card = Cards[index];
        Cards.RemoveAt(index);
        Debug.Log($"Playing Card {card}");
        CardGame.PlayCard(this, card);
    }
    public void DrawNormalCard()
    {
        var card = CardGame.DrawCardFromNormalPool();
        AddCard(card);
    }
    public IEnumerable<int> GetPlayableCardIndexes()
    {
        for (int i = 0; i < Cards.Count; i++)
        {
            Card card = Cards[i];
            if (CardGame.IsPlayableCard(card))
            {
                yield return i;
            }
        }
    }
    public IEnumerable<Card> GetAllCards()
    => Cards;
    public IEnumerable<(Card card, int index)> GetPlayableCards()
    {
        foreach (var index in GetPlayableCardIndexes())
        {
            yield return (Cards[index], index);
        }
    }
    public int CardCount() => Cards.Count;
    public Card GetCard(int index)
    {
        return Cards[index];
    }
    public void ClearCards()
    {
        Cards = new();
    }
    public void DiscardCard(Card card)
    {
        int index = Cards.IndexOf(card);
        DiscardCard(index);
    }
    public void DiscardCard(int index)
    {
        var card = Cards[index];
        Cards.RemoveAt(index);
        CardGame.DiscardCard(card);
    }
    public bool IsPlayableCard(Card card)
    {
        return CardGame.IsPlayableCard(card);
    }
}
public interface PlayerBehaviour
{
    public Player GetPlayer();
    public void StartTurn(Action onTurnEnd);
    public void DiscardCards(Action onDiscardEnd, int quantity);
    // public void SelectCards(Action<IEnumerable<Card>> OnCardSelect, int quantity, string message);
}