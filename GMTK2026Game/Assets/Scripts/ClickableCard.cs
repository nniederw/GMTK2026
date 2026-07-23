using System;
using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
public class ClickableCard : VisualCard
{
    private BoxCollider2D BoxCollider2D;
    private event Action<ClickableCard> OnCardClick = delegate { };
    public void SubscribeOnCardClick(Action<ClickableCard> action)
    {
        OnCardClick += action;
    }
    private void OnMouseDown()
    {
        OnCardClick.Invoke(this);
    }
    protected override void Start()
    {
        base.Start();
        BoxCollider2D = GetComponent<BoxCollider2D>();
    }
    protected override void Update()
    {
        base.Update();
        FitColliderFast();
        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }
    }
    [ContextMenu("Fit Collider")]
    public void FitCollider()
    {
        var collider = GetComponent<BoxCollider2D>();
        var spriteRenderer = GetComponent<SpriteRenderer>();
        FitCollider(collider, spriteRenderer);
    }
    public void FitColliderFast() => FitCollider(BoxCollider2D, SpriteRenderer);
    public void FitCollider(BoxCollider2D collider, SpriteRenderer spriteRenderer)
    {
        Sprite sprite = spriteRenderer.sprite;
        if (sprite == null)
        {
            return;
        }
        collider.size = sprite.bounds.size;
        collider.offset = sprite.bounds.center;
    }
}
[RequireComponent(typeof(SpriteRenderer))]
public class VisualCard : MonoBehaviour
{
    public Card Card = null;
    public bool HideCard = false;
    [SerializeField] private Color HighlightColor = new Color(1f, 1f, 1f);
    [SerializeField] private Color UnhighlightColor = new Color(0.8f, 0.8f, 0.8f);
    protected SpriteRenderer SpriteRenderer;
    public bool Highlighted = false;
    protected virtual void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }
    protected virtual void Update()
    {
        if (Card != null)
        {
            SpriteRenderer.sprite = HideCard ? Card.BackSideSprite : Card.Sprite;
            SpriteRenderer.color = Highlighted ? HighlightColor : UnhighlightColor;
        }
    }
}