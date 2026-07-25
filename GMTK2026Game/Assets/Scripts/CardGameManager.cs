using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
public class CardGameManager : MonoBehaviour
{
    [SerializeField] private ClickableCard ClickableCardPrefab;
    private ClickableCard PlayStackTopCard;
    private ClickableCard DrawStackTopCard;
    [SerializeField] private Vector2 LocalOffsetDrawStack;
    [SerializeField] private Vector2 LocalOffsetPlayStack;
    private List<PlayerBehaviour> PlayersInOrder;
    private Dictionary<Player, PlayerBehaviour> PlayerBehaviourMapping;
    private int Direction = 1;
    private int CurrentPlayer = 0;
    public CardGame Game;
    public static CardGameManager Instance;
    private event Action OnPlayStackClick = delegate { };
    private event Action OnDrawStackClick = delegate { };
    private bool AfterTurn = false;
    private float TimeSinceTurnEnd = 0f;
    private float TimeInbetweenTurns = 1f;
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
    private void Update()
    {
        if (!AfterTurn)
        {
            return;
        }
        TimeSinceTurnEnd += Time.deltaTime;
        if (TimeSinceTurnEnd >= TimeInbetweenTurns)
        {
            ContinueTurnEnd();
            AfterTurn = false;
        }
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
    public static void SetPlayers(IEnumerable<PlayerBehaviour> players)
    {
        Instance.PlayersInOrder = players.ToList();
        Instance.PlayerBehaviourMapping = new();
        foreach (var pb in Instance.PlayersInOrder)
        {
            Instance.PlayerBehaviourMapping[pb.GetPlayer()] = pb;
        }
        Instance.CurrentPlayer = -1;
        Instance.Direction = 1;
        OnTurnEnd();
    }
    public static void PlayerDiscard(PlayerIdentifier players, int quantity)
    {
        if (players.Players.Any())
        {
            var newIdf = new PlayerIdentifier(players.Players.Skip(1), players.NextPlayers);
            var player = players.Players.First();
            Instance.PlayerBehaviourMapping[player].DiscardCards(() => PlayerDiscard(newIdf, quantity), quantity);
        }
        if (players.NextPlayers.Any())
        {
            var newIdf = new PlayerIdentifier(players.Players, players.NextPlayers.Skip(1));
            var player = NextXPlayerIndex(players.NextPlayers.First());
            Instance.PlayersInOrder[player].DiscardCards(() => PlayerDiscard(newIdf, quantity), quantity);
        }
    }
    private static int NextXPlayerIndex(int x)
    {
        x *= Instance.Direction;
        return BetterMath.RealMod(Instance.CurrentPlayer + x, Instance.PlayersInOrder.Count);
    }
    private static void OnTurnEnd()
    {
        Instance.AfterTurn = true;
        Instance.TimeSinceTurnEnd = 0f;
        // Instance.Invoke(nameof(ContinueTurnEnd), 1f);
    }
    private static void ContinueTurnEnd()
    {
        Instance.CurrentPlayer = NextXPlayerIndex(1);
        Instance.PlayersInOrder[Instance.CurrentPlayer].StartTurn(OnTurnEnd);
    }
}