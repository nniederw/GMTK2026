using System.Collections.Generic;
using System.Linq;
public class PlayerIdentifier
{
    public List<Player> Players;
    public List<int> NextPlayers; //accepts +1 +2 +3... & -1 -2 -3... + meaning in the playing direction
    public PlayerIdentifier(IEnumerable<Player> players = null, IEnumerable<int> nextPlayers = null)
    {
        players ??= new List<Player>();
        nextPlayers ??= new List<int>();
        Players = players.ToList();
        NextPlayers = nextPlayers.ToList();
    }
}