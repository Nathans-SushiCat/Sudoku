using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ButtonAnimation : MonoBehaviour
{
    Animator animator;
    float timer = 0;

    private void Start()
    {
        animator = GetComponent<Animator>();
        //animator.speed = 2;
    }
    private void OnMouseDown()
    {
        animator.SetBool("Highlighted", false);
    }
    /*

    private void OnMouseOver()
    {
        Debug.Log("OVER");
        timer += Time.deltaTime;
        if(timer > 1f/animator.speed)
        {
            animator.SetBool("Highlight", true);
            animator.speed = 0;
        }
    }
    private void OnMouseExit()
    {
        Debug.Log("Down");
        animator.SetBool("Highlight", false);
        animator.speed = 5;
        timer = 0;
        animator.Play("Highlighted", 0, 60);
    }*/
}
