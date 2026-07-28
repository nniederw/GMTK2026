[System.Serializable]
public struct CardQuantityPair
{
    public Card Card;
    public int Quantity;
    public CardQuantityPair(Card card, int quantity)
    {
        Card = card;
        Quantity = quantity;
    }
}