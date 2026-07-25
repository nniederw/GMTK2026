using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class CardGameGenerator : MonoBehaviour
{
    [SerializeField] private List<GameObject> Players = new();
    [SerializeField] private CardPool NormalCardPool;
    [SerializeField] private CardPool JokerCardPool;
    [SerializeField] private Card StartStackCard;
    private CardGame CardGame;
    private void Start()
    {
        RegenerateGame();
    }
    [ContextMenu("Regenerate Game")]
    private void RegenerateGame()
    {
        CardGame = new CardGame(NormalCardPool.Cards, JokerCardPool.Cards, Players.Select(i => i.GetComponent<PlayerBehaviour>()).Select(i => i.GetPlayer()), StartStackCard);
        CardGameManager.SetPlayers(Players.Select(i => i.GetComponent<PlayerBehaviour>()));
    }
}