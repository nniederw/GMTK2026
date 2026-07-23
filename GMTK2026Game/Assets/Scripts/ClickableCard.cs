using UnityEngine;
[RequireComponent(typeof(Collider2D))]
public class ClickableCard : VisualCard
{
    private void OnMouseDown()
    {
        Debug.Log("Sprite clicked");
    }
    private void Update()
    {
        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }
    }
}
[RequireComponent(typeof(SpriteRenderer))]
public class VisualCard : MonoBehaviour
{
    [SerializeField] private Card Card = null;
    private SpriteRenderer SpriteRenderer;
    public void SetCard(Card card)
    {
        Card = card;
    }
    private void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        if (Card != null)
        {
            SpriteRenderer.sprite = Card.Sprite;
        }
        if ()
    }
}