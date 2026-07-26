using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class CardGame
{
    public const int StartNormalCardCount = 5;
    public const int StartJokerCardCount = 1;
    private List<Player> Players = new();
    public IReadOnlyList<Player> GetPlayers => Players;
    private Queue<Card> NormalCardPool;
    private Queue<Card> JokerCardPool;
    public Card LastPlayedCard { get; private set; }
    public List<Card> PlayedCards;//todo
    private HashSet<Card> NormalCards;
    private HashSet<Card> JokerCards;
    public CardGame(IEnumerable<CardQuantityPair> cards, IEnumerable<CardQuantityPair> jokerCards, IEnumerable<Player> players, Card startLastPlayedCard)
    {
        Players = players.ToList();
        NormalCardPool = new();
        JokerCardPool = new();
        PlayedCards = new();
        FillQueue(cards, NormalCardPool);
        FillQueue(jokerCards, JokerCardPool);
        NormalCards = cards.Select(i => i.Card).ToHashSet();
        JokerCards = jokerCards.Select(i => i.Card).ToHashSet();
        if (NormalCards.Intersect(JokerCards).Any())
        {
            Debug.Log($"Cards in {nameof(NormalCards)} & {nameof(JokerCards)} are not mutually exclusive.");
        }
        LastPlayedCard = startLastPlayedCard;
        CardGameManager.SetPlayStackCard(LastPlayedCard);
        InitializePlayers();
    }
    private void FillQueue(IEnumerable<CardQuantityPair> cards, Queue<Card> toFill)
    {
        var flattenedCards = cards.SelectMany(i => Enumerable.Repeat(i.Card, i.Quantity)).ToList();
        FillQueue(flattenedCards, toFill);
    }
    private void FillQueue(IEnumerable<Card> cards, Queue<Card> toFill)
    {
        var randomOrderCards = RandomUtils.RandomlyReorderList(cards.ToList());
        foreach (var card in randomOrderCards)
        {
            toFill.Enqueue(card);
        }
    }
    private void InitializePlayers()
    {
        foreach (Player player in Players)
        {
            player.ClearCards();
            player.CardGame = this;
            InitializeStartingHand(player);
        }
    }
    private void InitializeStartingHand(Player player)
    {
        for (int i = 0; i < StartNormalCardCount; i++)
        {
            player.AddCard(DrawCardFromNormalPool());
        }
        for (int i = 0; i < StartJokerCardCount; i++)
        {
            player.AddCard(DrawCardFromJokerPool());
        }
    }
    public void SetLastPlayedCard(Card card)
    {
        LastPlayedCard = card;
    }
    public void PlayCard(Player player, Card card, Action onFinishPlay)
    {
        PlayedCards.Add(card);
        if (card.CardType == CardType.Number)
        {
            if (card.Number == 0)
            {
                if (CardGameManager.Instance.PlayerBehaviourMapping[player].PlayerName == "You")
                {
                    CardGameManager.Instance.ActivateWin();
                }
                else
                {
                    CardGameManager.Instance.ActivateLose();
                }
            }
            LastPlayedCard = card;
            CardGameManager.SetPlayStackCard(LastPlayedCard);
            player.AddCard(DrawCardFromNormalPool());
            player.AddCard(DrawCardFromJokerPool());
            onFinishPlay();
            return;
        }
        if (card.CardType == CardType.Joker)
        {
            switch (card.JokerType)
            {
                case JokerType.PotOfGreed:
                    player.AddCard(DrawCardFromNormalPool());
                    player.AddCard(DrawCardFromJokerPool());
                    onFinishPlay();
                    break;
                case JokerType.RedHerring:
                    onFinishPlay();
                    break;
                case JokerType.Taxes:
                    CardGameManager.PlayerDiscard(new PlayerIdentifier(nextPlayers: new List<int> { 1 }), 2, onFinishPlay);
                    break;
            }
            return;
        }
        if (card.CardType == CardType.Special)
        {
            switch (card.SpecialType)
            {
                case SpecialType.Skip:
                    CardGameManager.SelectPlayers(player,
                    (player) => { CardGameManager.SkipPlayer(player.First()); onFinishPlay(); }, 1);
                    break;
                case SpecialType.Reverse:
                    CardGameManager.Instance.Direction *= -1;
                    onFinishPlay();
                    break;
                case SpecialType.Steal:
                    CardGameManager.SelectPlayers(player,
                    (players) => { player.AddCard(players.First().RemoveRandomCards(1).First()); onFinishPlay(); },
                     1);
                    break;
                    // case SpecialType.Subtract:
                    //     CardGameManager.SelectCards(player, (cards) =>
                    //     {
                    //         var c = cards.First();
                    //         c
                    //     }
            }
        }
        if (card.CardType == CardType.Event)
        {
            switch (card.EventType)
            {
                case EventType.Christmas:

                    break;
                case EventType.Communism:
                    break;
                case EventType.Friday13th:
                    foreach (var rcard in player.RemoveRandomCards(player.CardCount()))
                    {
                        PlayedCards.Add(rcard);
                    }
                    InitializeStartingHand(player);
                    break;
                case EventType.RobinHood:

                    break;
                case EventType.Inflation:
                    foreach (var p in Players)
                    {
                        var oldCards = p.GetAllCards().ToList();
                        p.ClearCards();
                        foreach (var ocard in oldCards)
                        {
                            Card newCard = ocard;
                            if (ocard.CardType == CardType.Number)
                            {
                                newCard = ocard.PreviousNumber;
                            }
                            p.AddCard(newCard);
                        }
                    }
                    break;
            }
        }
    }
    public void DiscardCard(Card card)
    {
        PlayedCards.Add(card);
    }
    public bool IsPlayableCard(Card card)
    {
        if (card.CardType == CardType.Number)
        {
            return LastPlayedCard.Number - 1 == card.Number;
        }
        return true;
    }
    public bool HasCardsInNormalPool()
    {
        return NormalCardPool.Any();
    }
    public bool HasCardsInJokerPool()
    {
        return JokerCardPool.Any();
    }
    public Card DrawCardFromNormalPool()
    {
        if (HasCardsInNormalPool())
        {
            return NormalCardPool.Dequeue();
        }
        if (ShuffleNormalCardsBackIn())
        {
            return DrawCardFromNormalPool();
        }
        throw new System.Exception("No cards left in normal pool.");
    }
    public Card DrawCardFromJokerPool()
    {
        if (HasCardsInJokerPool())
        {
            return JokerCardPool.Dequeue();
        }
        if (ShuffleJokerCardsBackIn())
        {
            return DrawCardFromJokerPool();
        }
        throw new System.Exception("No cards left in joker pool.");
    }
    public bool ShuffleNormalCardsBackIn()
    {
        List<Card> normalCards = new();
        List<Card> nonNormalCards = new();
        foreach (var card in PlayedCards)
        {
            if (NormalCards.Contains(card))
            {
                normalCards.Add(card);
                continue;
            }
            nonNormalCards.Add(card);
        }
        if (!normalCards.Any())
        {
            return false;
        }
        FillQueue(normalCards, NormalCardPool);
        PlayedCards = nonNormalCards;
        return true;
    }
    public bool ShuffleJokerCardsBackIn()
    {
        List<Card> jokerCards = new();
        List<Card> nonJokerCards = new();
        foreach (var card in PlayedCards)
        {
            if (JokerCards.Contains(card))
            {
                jokerCards.Add(card);
                continue;
            }
            nonJokerCards.Add(card);
        }
        if (!jokerCards.Any())
        {
            return false;
        }
        FillQueue(jokerCards, JokerCardPool);
        PlayedCards = nonJokerCards;
        return true;
    }
}