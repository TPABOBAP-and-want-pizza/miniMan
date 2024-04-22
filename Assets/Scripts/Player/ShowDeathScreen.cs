using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowDeathScreen : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject deadScreen;
    [SerializeField] private GameObject pressKey;


    public void Die(bool skipAnimation)
    {
        animator.Play("Death");

        //if(skipAnimation)
            ShowDeath();
    }

    public void ShowDeath()
    {
        Debug.Log("You Dead");
        deadScreen.SetActive(true);
    }

    public void ShowPressKey()
    {
        if (pressKey == null)
        {
            Debug.LogError("PressKey is not assigned in the inspector!");
        }
        pressKey.SetActive(true);
    }

    public void HideDeath()
    {
        pressKey.SetActive(false);
        deadScreen.SetActive(false);
    }
}
