
using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerButton : MonoBehaviour
{
    [SerializeField] private TMP_Text Text;
    private Action OnClick;
    private void Awake()
    {
        if (Text == null) { throw new Exception($"{nameof(Text)} was null on {nameof(PlayerButton)}"); }
    }
    public void SetValues(string text, Action onClick)
    {
        OnClick = onClick;
        Text.text = text;
    }
    private void OnMouseDown()
    {
        OnClick();
    }
}