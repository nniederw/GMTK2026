using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
public class ComputerPlayer : MonoBehaviour, PlayerBehaviour
{
    public string PlayerName => name;
    [SerializeField] private VisualCard CardPrefab;
    [SerializeField] private PlayerIndicator Indicator;
    private Player Player = new Player();
    private Action OnTurnEnd;
    private int DiscardCardAmount = 0;
    private Action OnDiscardEnd;
    private TurnAction CurrentTurnAction = TurnAction.None;
    private void Start()
    {
        Indicator.AssignPlayer(Player);
        Indicator.SetName(name);
    }
    private void Update()
    {
        switch (CurrentTurnAction)
        {
            case TurnAction.NormalTurn:
                CurrentTurnAction = TurnAction.None;
                DoTurn();
                break;
            case TurnAction.DiscardCards:
                CurrentTurnAction = TurnAction.None;
                DoDiscardCards();
                OnDiscardEnd.Invoke();
                break;
        }
    }
    private void DoTurn()
    {
        int cardCount = Player.CardCount();
        List<(Card card, int index)> cards = new();
        foreach (var index in Player.GetPlayableCardIndexes())
        {
            cards.Add((Player.GetCard(index), index));
        }
        var numberCards = cards.Where(i => i.card.CardType == CardType.Number).ToList();
        if (numberCards.Any())
        {
            Player.PlayCard(numberCards.First().index, OnTurnEnd);
            return;
        }
        var nonNumberCards = cards.Where(i => i.card.CardType != CardType.Number).ToList();
        if (nonNumberCards.Any())
        {
            var card = nonNumberCards[new System.Random().Next(nonNumberCards.Count)];
            Player.PlayCard(card.index, OnTurnEnd);
            return;
        }
        Player.DrawNormalCard();
        OnTurnEnd();
    }
    private void DoDiscardCards()
    {
        while (Player.CardCount() > 0 && DiscardCardAmount > 0)
        {
            DiscardCardAmount--;
            var cards = Player.GetAllCards();
            var numberCards = cards.Where(i => i.CardType == CardType.Number);
            var duplicateNumbers = numberCards.Except(numberCards.Distinct()).ToList();
            if (duplicateNumbers.Any())
            {
                Player.DiscardCard(duplicateNumbers.First());
                continue;
            }
            if (numberCards.Any())
            {
                var sortedNumbers = numberCards.ToList();
                sortedNumbers.Sort((a, b) => b.Number.CompareTo(a.Number));
                Player.DiscardCard(sortedNumbers.First());
                continue;
            }
            if (cards.Any())
            {
                var card = cards.First();
                Player.DiscardCard(card);
            }
        }
    }
    public Player GetPlayer()
    {
        return Player;
    }
    public void StartTurn(Action onTurnEnd)
    {
        CurrentTurnAction = TurnAction.NormalTurn;
        OnTurnEnd = onTurnEnd;
        Debug.Log("Starting turn for computer");
    }
    public void DiscardCards(Action onDiscardEnd, int quantity)
    {
        CurrentTurnAction = TurnAction.DiscardCards;
        OnDiscardEnd = onDiscardEnd;
        DiscardCardAmount = quantity;
    }

    public void SelectPlayers(Action<IEnumerable<Player>> onSelectPlayerEnd, int quantity)
    {
        IEnumerable<Player> otherPlayers = Player.CardGame.GetPlayers.Except(Player);
        var result = RandomUtils.RandomSubset(otherPlayers, quantity);
        onSelectPlayerEnd.Invoke(result);
    }

    public void SelectCards(Action<IEnumerable<Card>> OnCardSelect, int quantity, string message)
    {
        var cards = Player.GetAllCards().ToList();
        quantity = Math.Max(quantity, cards.Count);
        var result = RandomUtils.RandomSubset(cards, quantity);
        OnCardSelect(result);
    }
}