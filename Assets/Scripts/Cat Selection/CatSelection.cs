using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

public class CatSelection : MonoBehaviour, IPointerClickHandler
{
    private bool isSelected;
    private Animator anim;
    
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    { 
        // this.anim.speed = this.isSelected ? 1 : 0;
    }

    public void ToggleSelected()
    {
        this.isSelected = !this.isSelected;
        Debug.Log($"{gameObject.name} is selected: {this.isSelected}");
    }
    
    
    [UsedImplicitly]
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        //Output to console the clicked GameObject's name and the following message. You can replace this with your own actions for when clicking the GameObject.
        Debug.Log(name + " Game Object Clicked!");
    }}
