using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
public class PlayerControler : MonoBehaviour, PlayerBehaviour
{
    public string PlayerName => name;
    [SerializeField] private ClickableCard CardPrefab;
    [SerializeField] private PlayerSelectionWheel PlayerSelectionWheelPrefab;
    private Player Player = new Player();
    private List<ClickableCard> CardPrefabPool = new();
    private const int StartPoolSize = 5;
    private ClickableCard HighlightedCard = null;
    private float CardLength = 1.45f;
    private Action OnTurnEnd;
    private Action OnDiscardingEnd;
    Action<IEnumerable<Card>> OnCardSelect;
    private TurnAction CurrentTurnAction = TurnAction.None;
    private int ToDiscardQuantity = 0;
    private int ToSelectQuantity = 0;
    public Player GetPlayer()
    {
        return Player;
    }
    public void StartTurn(Action onTurnEnd)
    {
        CurrentTurnAction = TurnAction.NormalTurn;
        OnTurnEnd = onTurnEnd;
        Debug.Log("Starting turn for player");
    }
    public void DiscardCards(Action onDiscardEnd, int quantity)
    {
        CurrentTurnAction = TurnAction.DiscardCards;
        ToDiscardQuantity = quantity;
        OnDiscardingEnd = onDiscardEnd;
    }
    private void Start()
    {
        for (int i = 0; i < StartPoolSize; i++)
        {
            AddClickableCardToPool();
        }
        CardGameManager.SubscribeOnPlayStackClick(OnPlayStackClick);
        CardGameManager.SubscribeOnDrawStackClick(OnDrawStackClick);
    }
    private void Update()
    {
        int cardCount = Player.CardCount();
        for (int i = 0; i < cardCount; i++)
        {
            if (CardPrefabPool.Count == i)
            {
                AddClickableCardToPool();
            }
            CardPrefabPool[i].Card = Player.GetCard(i);
            CardPrefabPool[i].gameObject.SetActive(true);
            CardPrefabPool[i].SetSpriteSortingOrder(i);
        }
        for (int i = cardCount; i < CardPrefabPool.Count; i++)
        {
            CardPrefabPool[i].gameObject.SetActive(false);
        }
        float cardPos = -(cardCount - 1) / 2f * CardLength;
        for (int i = 0; i < cardCount; i++)
        {
            CardPrefabPool[i].transform.localPosition = new Vector3(cardPos, 0f, i);
            cardPos += CardLength;
        }
    }
    private void OnPlayStackClick()
    {
        if (HighlightedCard == null)
        {
            return;
        }
        switch (CurrentTurnAction)
        {
            case TurnAction.NormalTurn:
                if (Player.IsPlayableCard(HighlightedCard.Card))
                {
                    CurrentTurnAction = TurnAction.None;
                    Player.PlayCard(HighlightedCard.Card, OnTurnEnd);
                }
                break;
            case TurnAction.DiscardCards:
                Player.DiscardCard(HighlightedCard.Card);
                ToDiscardQuantity--;
                if (ToDiscardQuantity == 0)
                {
                    EndDiscarding();
                }
                break;
        }
    }
    private void EndTurn()
    {
        CurrentTurnAction = TurnAction.None;
        OnTurnEnd.Invoke();
    }
    private void EndDiscarding()
    {
        CurrentTurnAction = TurnAction.None;
        OnDiscardingEnd.Invoke();
    }
    private void OnDrawStackClick()
    {
        if (CurrentTurnAction != TurnAction.NormalTurn)
        {
            return;
        }
        Player.DrawNormalCard();
        EndTurn();
    }
    private void AddClickableCardToPool()
    {
        var card = Instantiate(CardPrefab, transform);
        card.SubscribeOnCardClick(OnCardClick);
        CardPrefabPool.Add(card);
    }
    private void OnCardClick(ClickableCard card)
    {
        if (CurrentTurnAction != TurnAction.NormalTurn)
        {
            return;
        }
        if (HighlightedCard == null)
        {
            if (CurrentTurnAction == TurnAction.SelectCards)
            {
                CurrentTurnAction = TurnAction.None;
                List<Card> cards = new List<Card> { card.Card };
                OnCardSelect(cards);
            }
            HighlightedCard = card;
            card.Highlighted = true;
            return;
        }
        if (HighlightedCard == card)
        {
            HighlightedCard = null;
            card.Highlighted = false;
            return;
        }
        HighlightedCard.Highlighted = false;
        HighlightedCard = card;
        card.Highlighted = true;
    }
    public void SelectPlayers(Action<IEnumerable<Player>> onSelectPlayerEnd, int quantity)
    {
        var playerSelection = Instantiate(PlayerSelectionWheelPrefab);
        var players = CardGameManager.Instance.GetPlayers.ToList();
        var playerNames = players.Select(i => (i.GetPlayer(), i.PlayerName));
        playerSelection.GenerateWheel(playerNames, onSelectPlayerEnd, quantity);
    }

    public void SelectCards(Action<IEnumerable<Card>> onCardSelect, int quantity, string message)
    {
        CurrentTurnAction = TurnAction.DiscardCards;
        ToDiscardQuantity = quantity;
        OnCardSelect = onCardSelect;
    }
}
public enum TurnAction
{
    None,
    NormalTurn,
    DiscardCards,
    SelectCards,
}