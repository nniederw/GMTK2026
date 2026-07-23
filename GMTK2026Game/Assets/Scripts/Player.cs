using System;
using System.Collections.Generic;
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
    public int CardCount() => Cards.Count;
    public Card GetCard(int index)
    {
        return Cards[index];
    }
    public void ClearCards()
    {
        Cards = new();
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
}