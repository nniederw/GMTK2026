using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class VisualCard : MonoBehaviour
{
    public Card Card = null;
    public bool HideCard = false;
    [SerializeField] private Color HighlightColor = new Color(1f, 1f, 1f);
    [SerializeField] private Color UnhighlightColor = new Color(0.8f, 0.8f, 0.8f);
    protected SpriteRenderer SpriteRenderer;
    public bool Highlighted = false;
    protected virtual void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }
    protected virtual void Start() { }
    protected virtual void Update()
    {
        if (Card != null)
        {
            SpriteRenderer.sprite = HideCard ? Card.BackSideSprite : Card.Sprite;
            SpriteRenderer.color = Highlighted ? HighlightColor : UnhighlightColor;
        }
    }
    public void SetSpriteSortingOrder(int order)
    {
        SpriteRenderer.sortingOrder = order;
    }
}