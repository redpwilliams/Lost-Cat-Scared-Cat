using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>Represents an individual Cat on the Cat Select screen</summary>
/// <remarks>
/// 
/// </remarks>
public class CatSelection : MonoBehaviour, IPointerClickHandler
{
    private bool _isSelected;
    [field: SerializeField]
    public uint Id { get; private set; }
    private Animator _anim;
    private SpriteRenderer _sr;
    private Sprite _defaultSprite;

    private void OnEnable()
    {
        EventManager.Events.OnCatSelect += HandleCatClicked;
    }

    private void OnDisable()
    {
        EventManager.Events.OnCatSelect -= HandleCatClicked;
    }

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        DisableAnimation();

        _sr = GetComponent<SpriteRenderer>();
        _defaultSprite = this._sr.sprite;

        var preferences = SaveSystem.LoadPreferences();
        if (Id == preferences.CatID) EnableAnimation();

    }

    private void HandleCatClicked(uint i)
    {
        // If cat is selected
        if (i == Id)
        {
            EnableAnimation();
            return;
        }
        
        // Else, stop animation and reset sprite
        DisableAnimation();
        _sr.sprite = this._defaultSprite;
    }

    private void EnableAnimation()
    {
        _anim.speed = 1;
    }

    private void DisableAnimation()
    {
        _anim.speed = 0;
    }

    [UsedImplicitly]
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        EventManager.Events.CatSelect(Id);
    }
}
