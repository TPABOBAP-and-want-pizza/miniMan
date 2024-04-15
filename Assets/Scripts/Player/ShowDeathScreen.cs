using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowDeathScreen : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject deadScreen;


    public void Die(bool skipAnimation)
    {
        animator.Play("Death");

        if(skipAnimation)
            ShowDeath();
    }

    public void ShowDeath()
    {
        Debug.Log("You Dead");
        deadScreen.active = true;
    }

    public void HideDeath()
    {
        deadScreen.active = false;
    }
}
