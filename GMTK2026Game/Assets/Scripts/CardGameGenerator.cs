using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class CardGameGenerator : MonoBehaviour
{
    [SerializeField] private List<GameObject> Players = new();
    [SerializeField] private CardPool NormalCardPool;
    private CardGame CardGame;
    private void Start()
    {
        RegenerateGame();
    }
    [ContextMenu("Regenerate Game")]
    private void RegenerateGame()
    {
        CardGame = new CardGame(NormalCardPool.Cards, Players.Select(i => i.GetComponent<PlayerContainer>()).Select(i => i.GetPlayer()));
    }
}