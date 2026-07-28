using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CardGameManager : MonoBehaviour
{
    [SerializeField] private ClickableCard ClickableCardPrefab;
    private ClickableCard PlayStackTopCard;
    private ClickableCard DrawStackTopCard;
    [SerializeField] private Vector2 LocalOffsetDrawStack;
    [SerializeField] private Vector2 LocalOffsetPlayStack;
    private List<PlayerBehaviour> PlayersInOrder;
    private List<bool> SkippedPlayers;
    public IReadOnlyList<PlayerBehaviour> GetPlayers => PlayersInOrder;
    public Dictionary<Player, PlayerBehaviour> PlayerBehaviourMapping;
    public int Direction = 1;
    private int CurrentPlayer = 0;
    public CardGame Game;
    public static CardGameManager Instance;
    private event Action OnPlayStackClick = delegate { };
    private event Action OnDrawStackClick = delegate { };
    private bool AfterTurn = false;
    private float TimeSinceTurnEnd = 0f;
    private float TimeInbetweenTurns = 1f;
    private static string SceneName = "Game";
    public GameObject YouWin;
    public GameObject YouLose;
    private void Awake()
    {
        Instance = this;
        PlayStackTopCard = Instantiate(ClickableCardPrefab, transform);
        DrawStackTopCard = Instantiate(ClickableCardPrefab, transform);
        PlayStackTopCard.SubscribeOnCardClick((_) => OnPlayStackClick.Invoke());
        DrawStackTopCard.SubscribeOnCardClick((_) => OnDrawStackClick.Invoke());
        PlayStackTopCard.transform.localPosition = LocalOffsetPlayStack;
        DrawStackTopCard.transform.localPosition = LocalOffsetDrawStack;
        DrawStackTopCard.HideCard = true;
    }
    private void Start()
    {

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
    public void ActivateWin()
    {
        YouWin.SetActive(true);
    }
    public void ActivateLose()
    {
        YouLose.SetActive(true);
    }
    public static void StartGame()
    {
        SceneManager.LoadScene(SceneName);
    }
    public static int PlayerIndex(Player player)
    {
        for (int i = 0; i < Instance.PlayersInOrder.Count; i++)
        {
            var p = Instance.PlayersInOrder[i];
            if (p.GetPlayer() == player)
            {
                return i;
            }
        }
        throw new Exception();
    }
    public static void SkipPlayer(Player player)
    {
        Instance.SkippedPlayers[PlayerIndex(player)] = true;
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
        Debug.Log($"Instance:{Instance}");
        Instance.PlayStackTopCard.Card = card;
    }
    public static void SetPlayers(IEnumerable<PlayerBehaviour> players)
    {
        Instance.PlayersInOrder = players.ToList();
        Instance.SkippedPlayers = Enumerable.Repeat(false, Instance.PlayersInOrder.Count).ToList();
        Instance.PlayerBehaviourMapping = new();
        foreach (var pb in Instance.PlayersInOrder)
        {
            Instance.PlayerBehaviourMapping[pb.GetPlayer()] = pb;
        }
        Instance.CurrentPlayer = -1;
        Instance.Direction = 1;
        OnTurnEnd();
    }
    public static void PlayerDiscard(PlayerIdentifier players, int quantity, Action onFinish)
    {
        if (players.Players.Any())
        {
            var newIdf = new PlayerIdentifier(players.Players.Skip(1), players.NextPlayers);
            var player = players.Players.First();
            Instance.PlayerBehaviourMapping[player].DiscardCards(() => PlayerDiscard(newIdf, quantity, () => { }), quantity);
        }
        if (players.NextPlayers.Any())
        {
            var newIdf = new PlayerIdentifier(players.Players, players.NextPlayers.Skip(1));
            var player = NextXPlayerIndex(players.NextPlayers.First());
            Instance.PlayersInOrder[player].DiscardCards(() => PlayerDiscard(newIdf, quantity, () => { }), quantity);
        }
        onFinish();
    }
    public static void SelectPlayers(Player player, Action<IEnumerable<Player>> onSelectPlayerEnd, int quantity)
    {
        Instance.PlayerBehaviourMapping[player].SelectPlayers(onSelectPlayerEnd, quantity);
    }
    public static void SelectCards(Player player, Action<IEnumerable<Card>> onSelectCardEnd, int quantity)
    {
        Instance.PlayerBehaviourMapping[player].SelectCards(onSelectCardEnd, quantity, "");
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
        if (Instance.SkippedPlayers[Instance.CurrentPlayer])
        {
            Instance.SkippedPlayers[Instance.CurrentPlayer] = false;
            ContinueTurnEnd();
        }
        Instance.PlayersInOrder[Instance.CurrentPlayer].StartTurn(OnTurnEnd);
    }
}