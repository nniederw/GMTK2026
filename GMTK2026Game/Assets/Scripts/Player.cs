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
    public Card GetCard(int index)
    {
        return Cards[index];
    }
}