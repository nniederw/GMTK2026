using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour, PlayerBehaviour
{
    [SerializeField] private ClickableCard CardPrefab;
    private Player Player = new Player();
    private List<ClickableCard> CardPrefabPool = new();
    private const int StartPoolSize = 5;
    private ClickableCard HighlightedCard = null;
    private float CardLength = 1.75f;

    private Action OnTurnEnd;
    private void Start()
    {
        for (int i = 0; i < StartPoolSize; i++)
        {
            AddClickableCardToPool();
        }
        CardGameManager.SubscribeOnPlayStackClick(OnPlayStackClick);
        CardGameManager.SubscribeOnDrawStackClick(OnDrawStackClick);
    }
    private void OnPlayStackClick()
    {
        if (HighlightedCard == null)
        {
            return;
        }
        Debug.Log($"Try playing card {HighlightedCard.Card}, playable: {Player.IsPlayableCard(HighlightedCard.Card)}");
        if (Player.IsPlayableCard(HighlightedCard.Card))
        {
            HighlightedCard.Highlighted = false;
            Player.PlayCard(HighlightedCard.Card);
        }
    }
    private void OnDrawStackClick()
    {
        Player.DrawNormalCard();
    }
    private void AddClickableCardToPool()
    {
        var card = Instantiate(CardPrefab, transform);
        card.SubscribeOnCardClick(OnCardClick);
        CardPrefabPool.Add(card);
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
        }
        for (int i = cardCount; i < CardPrefabPool.Count; i++)
        {
            CardPrefabPool[i].gameObject.SetActive(false);
        }
        float cardPos = -(cardCount - 1) / 2f * CardLength;
        for (int i = 0; i < cardCount; i++)
        {
            CardPrefabPool[i].transform.localPosition = new Vector2(cardPos, 0f);
            cardPos += CardLength;
        }
    }
    private void OnCardClick(ClickableCard card)
    {
        if (HighlightedCard == null)
        {
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
    public Player GetPlayer()
    {
        return Player;
    }
    public void StartTurn(Action onTurnEnd)
    {
        OnTurnEnd = onTurnEnd;
    }
}