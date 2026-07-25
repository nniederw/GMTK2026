using UnityEngine;
using TMPro;
public class PlayerIndicator : MonoBehaviour
{
    [SerializeField] private TMP_Text TextField;
    private Player Player;
    private string Name;
    public void SetName(string name)
    {
        Name = name;
    }
    public void AssignPlayer(Player player)
    {
        Player = player;
    }
    private void Start()
    {
        if (TextField == null)
        {
            throw new System.Exception($"{nameof(TextField)} was null on {nameof(PlayerIndicator)}");
        }
    }
    private void Update()
    {
        TextField.text = $"{Name}\n[{Player.CardCount()} Cards]";
    }
}