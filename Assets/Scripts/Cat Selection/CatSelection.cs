using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

public class CatSelection : MonoBehaviour, IPointerClickHandler
{
    private bool isSelected;
    [field: SerializeField]
    public int Id { get; private set; }
    private Animator anim;
    private SpriteRenderer sr;
    private Sprite defaultSprite;

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
        this.anim = GetComponent<Animator>();
        DisableAnimation();

        this.sr = GetComponent<SpriteRenderer>();
        this.defaultSprite = this.sr.sprite;
        
    }

    private void Start()
    {
        // TODO: Eventually, set starting cat through player preferences
        if (Id == 1) EnableAnimation();
    }

    private void HandleCatClicked(int i)
    {
        // If cat is selected
        if (i == Id)
        {
            EnableAnimation();
            return;
        }
        
        // Else, stop animation and reset sprite
        DisableAnimation();
        this.sr.sprite = this.defaultSprite;
    }

    private void EnableAnimation()
    {
        this.anim.speed = 1;
    }

    private void DisableAnimation()
    {
        this.anim.speed = 0;
    }

    [UsedImplicitly]
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        EventManager.Events.CatSelect(Id);
    }}
