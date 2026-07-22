using System.Collections.Generic;
public class Player
{
    private List<Card> Cards = new();
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
        Cards.RemoveAt(index);
        //TODO card effect
    }
}