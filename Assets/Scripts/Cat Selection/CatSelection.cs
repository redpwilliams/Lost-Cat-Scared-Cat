using System;
using UnityEngine;

public class CatSelection : MonoBehaviour
{
    [SerializeField]
    private bool isSelected;

    [SerializeField] private float speed = 1;
    private Animator anim;
    
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        this.anim.speed = this.speed;
    }
}
