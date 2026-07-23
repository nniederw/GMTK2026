using System.Collections.Generic;
using System.Linq;
public class CardGame
{
    public const int StartCardCount = 5;
    private List<Player> Players = new();
    private Queue<Card> NormalCardPool;
    public Card LastPlayedCard { get; private set; }
    public CardGame(IEnumerable<CardQuantityPair> cards, IEnumerable<Player> players)
    {
        Players = players.ToList();
        var flattenedCards = cards.SelectMany(i => Enumerable.Repeat(i.Card, i.Quantity)).ToList();
        var randomOrderCards = RandomUtils.RandomlyReorderList(flattenedCards);
        NormalCardPool = new();
        foreach (var card in randomOrderCards)
        {
            NormalCardPool.Enqueue(card);
        }
        InitializePlayers();
    }
    private void InitializePlayers()
    {
        foreach (Player player in Players)
        {
            player.CardGame = this;
            for (int i = 0; i < StartCardCount; i++)
            {
                player.AddCard(DrawCardFromNormalPool());
            }
        }
    }
    public void PlayCard(Player player, Card card)
    {
        if (card.CardType == CardType.Number)
        {
            LastPlayedCard = card;
        }
        //todo effect
    }
    public bool IsPlayableCard(Card card)
    {
        if (card.CardType == CardType.Number)
        {
            return LastPlayedCard.Number == card.Number - 1;
        }
        return true;
    }
    public bool HasCardsInNormalPool()
    {
        return NormalCardPool.Any();
    }
    public Card DrawCardFromNormalPool()
    {
        return NormalCardPool.Dequeue();
    }
}