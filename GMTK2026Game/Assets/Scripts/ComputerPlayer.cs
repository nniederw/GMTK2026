using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class ComputerPlayer : MonoBehaviour, PlayerBehaviour
{
    [SerializeField] private VisualCard CardPrefab;
    private Player Player = new Player();
    // private List<VisualCard> CardPrefabPool = new();
    // private const int StartPoolSize = 5;
    // private ClickableCard HighlightedCard = null;
    // private float CardLength = 1.45f;
    private Action OnTurnEnd;
    private void Start()
    {
        // for (int i = 0; i < StartPoolSize; i++)
        // {
        //     AddClickableCardToPool();
        // }
        // CardGameManager.SubscribeOnPlayStackClick(OnPlayStackClick);
        // CardGameManager.SubscribeOnDrawStackClick(OnDrawStackClick);
    }
    // private void OnPlayStackClick()
    // {
    //     if (HighlightedCard == null)
    //     {
    //         return;
    //     }
    //     Debug.Log($"Try playing card {HighlightedCard.Card}, playable: {Player.IsPlayableCard(HighlightedCard.Card)}");
    //     if (Player.IsPlayableCard(HighlightedCard.Card))
    //     {
    //         HighlightedCard.Highlighted = false;
    //         Player.PlayCard(HighlightedCard.Card);
    //     }
    // }
    // private void OnDrawStackClick()
    // {
    //     Player.DrawNormalCard();
    // }
    // private void AddClickableCardToPool()
    // {
    //     var card = Instantiate(CardPrefab, transform);
    //     card.SubscribeOnCardClick(OnCardClick);
    //     CardPrefabPool.Add(card);
    // }
    private void Update()
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
            Player.PlayCard(numberCards.First().index);
            return;
        }
        var nonNumberCards = cards.Where(i => i.card.CardType != CardType.Number).ToList();
        if (nonNumberCards.Any())
        {
            var card = nonNumberCards[new System.Random().Next(nonNumberCards.Count)];
            Player.PlayCard(card.index);
            return;
        }
        return; //todo
    }
    public Player GetPlayer()
    {
        return Player;
    }
    public void StartTurn(Action onTurnEnd)
    {
        OnTurnEnd = onTurnEnd;
    }
}