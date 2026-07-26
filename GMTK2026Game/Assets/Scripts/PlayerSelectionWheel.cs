using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
public class PlayerSelectionWheel : MonoBehaviour
{
    [SerializeField] private PlayerButton PlayerButtonPrefab;
    [SerializeField] private float RingSize = 2f;
    private int PlayersToSelect;
    private List<Player> SelectedPlayers = new();
    private Action<IEnumerable<Player>> OnPlayersSelect;
    [ContextMenu("GenerateWheelTest")]
    private void GenerateWheelTest()
    => GenerateWheel(new List<(Player, string)> { (new Player(), "P1"), (new Player(), "P2"), (new Player(), "P3"), (new Player(), "P4") }, (_) => { }, 1);
    public void GenerateWheel(IEnumerable<(Player, string)> players, Action<IEnumerable<Player>> onPlayersSelect, int quantity)
    {
        PlayersToSelect = quantity;
        OnPlayersSelect = onPlayersSelect;
        var playerList = players.ToList();
        var toInstantiate = playerList.Zip(PointsOnCircle(playerList.Count, RingSize), (a, b) => (a, b));
        foreach (var buttons in toInstantiate)
        {
            var obj = Instantiate(PlayerButtonPrefab, transform);
            obj.SetValues(buttons.a.Item2, () => ClickOnPlayer(buttons.a.Item1));
            obj.transform.localPosition = buttons.b;
        }
    }
    private void ClickOnPlayer(Player player)
    {
        SelectedPlayers.Add(player);
        if (SelectedPlayers.Count == PlayersToSelect)
        {
            OnPlayersSelect(SelectedPlayers);
            Destroy(gameObject);
        }
    }
    private static IEnumerable<Vector3> PointsOnCircle(int n, float radius, Vector3 center = new Vector3())
    {
        for (int i = 0; i < n; i++)
        {
            float angle = 2f * Mathf.PI * i / n;
            var point = center + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0f);
            yield return point;
        }
    }
}