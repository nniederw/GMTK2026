using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
public class CardGame
{
    public const int StartCardCount = 5;
    private List<Player> Players = new();
    private Queue<Card> NormalCardPool;
    public Card LastPlayedCard { get; private set; }
    public CardGame(IEnumerable<CardQuantityPair> cards, IEnumerable<Player> players, Card startLastPlayedCard)
    {
        Players = players.ToList();
        var flattenedCards = cards.SelectMany(i => Enumerable.Repeat(i.Card, i.Quantity)).ToList();
        var randomOrderCards = RandomUtils.RandomlyReorderList(flattenedCards);
        NormalCardPool = new();
        foreach (var card in randomOrderCards)
        {
            NormalCardPool.Enqueue(card);
        }
        LastPlayedCard = startLastPlayedCard;
        CardGameManager.SetPlayStackCard(LastPlayedCard);
        InitializePlayers();
    }
    private void InitializePlayers()
    {
        foreach (Player player in Players)
        {
            player.ClearCards();
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
            CardGameManager.SetPlayStackCard(LastPlayedCard);
        }
        //todo effect
    }
    public bool IsPlayableCard(Card card)
    {
        if (card.CardType == CardType.Number)
        {
            // if (LastPlayedCard == null)
            // {
            //     return card.Number == 9;
            // }
            UnityEngine.Debug.Log($"LastPlayedCard: {LastPlayedCard}, checking card: {card}");
            return LastPlayedCard.Number - 1 == card.Number;
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