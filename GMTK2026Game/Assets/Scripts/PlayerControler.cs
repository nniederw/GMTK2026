using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour, PlayerContainer
{
    [SerializeField] private ClickableCard CardPrefab;
    private Player Player = new Player();
    private List<ClickableCard> CardPrefabPool = new();
    private const int StartPoolSize = 5;
    private ClickableCard HighlightedCard = null;
    private float CardLength = 2f;
    private void Start()
    {
        for (int i = 0; i < StartPoolSize; i++)
        {
            AddClickableCardToPool();
        }
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
}