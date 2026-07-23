using System;
using System.Collections.Generic;
using UnityEngine;
public class CardGameManager : MonoBehaviour
{
    [SerializeField] private ClickableCard ClickableCardPrefab;
    private ClickableCard PlayStackTopCard;
    private ClickableCard DrawStackTopCard;
    [SerializeField] private Vector2 LocalOffsetDrawStack;
    [SerializeField] private Vector2 LocalOffsetPlayStack;
    private List<PlayerBehaviour> PlayersInOrder;
    private int direction = 1;
    public CardGame Game;
    public static CardGameManager Instance;
    private event Action OnPlayStackClick = delegate { };
    private event Action OnDrawStackClick = delegate { };
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        PlayStackTopCard = Instantiate(ClickableCardPrefab, transform);
        DrawStackTopCard = Instantiate(ClickableCardPrefab, transform);
        PlayStackTopCard.SubscribeOnCardClick((_) => OnPlayStackClick.Invoke());
        DrawStackTopCard.SubscribeOnCardClick((_) => OnDrawStackClick.Invoke());
        PlayStackTopCard.transform.localPosition = LocalOffsetPlayStack;
        DrawStackTopCard.transform.localPosition = LocalOffsetDrawStack;
        DrawStackTopCard.HideCard = true;
    }
    public static void SubscribeOnPlayStackClick(Action action)
    {
        Instance.OnPlayStackClick += action;
    }
    public static void SubscribeOnDrawStackClick(Action action)
    {
        Instance.OnDrawStackClick += action;
    }
    public static void SetPlayStackCard(Card card)
    {
        Instance.PlayStackTopCard.Card = card;
    }
}