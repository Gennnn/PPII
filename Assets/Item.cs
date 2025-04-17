using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    protected Animator animator;
    protected Renderer meshRenderer;
    protected string currentAnimationState;


    void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
        meshRenderer = GetComponent<Renderer>();
    }
    public virtual void Primary() { }
    public virtual void Secondary() { }

    protected void ChangeAnimationState(string newState)
    {
        currentAnimationState = newState;
        animator.CrossFadeInFixedTime(currentAnimationState, 0.1f);
    }
}
