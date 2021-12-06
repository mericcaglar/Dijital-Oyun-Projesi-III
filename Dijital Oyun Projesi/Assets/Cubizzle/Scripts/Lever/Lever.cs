using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Lever : MonoBehaviour
{
    public static event Action<int> leverObjectOnActivate;
    public static event Action<int> leverObjectOnDisable;

    bool leverStatus, characterStatus;
    [SerializeField] int id;
    [SerializeField] private Animator animator;



    private void Awake()
    {
        leverStatus = false;
        characterStatus = false;
    }

    private void Start()
    {
        LeverDisable();
    }

    private void OnMouseDown()
    {
        if (characterStatus)
            if (!leverStatus)
                LeverActivate();
            else
                LeverDisable();
            
    }


    void LeverActivate()
    {
        
        leverStatus = true;
        animator.SetTrigger("LeverOpen");
        StartCoroutine(LeverActiveOrDisable());
    }

    void LeverDisable()
    {
        leverStatus = false;
        animator.SetTrigger("LeverClose");
        StartCoroutine(LeverActiveOrDisable());
    }

    IEnumerator LeverActiveOrDisable()
    {
        yield return new WaitForSeconds(.5f);
        if (leverStatus)
        {
            leverObjectOnActivate?.Invoke(id);
        }
        else
        {
            leverObjectOnDisable?.Invoke(id);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.transform.tag);
        if (other.CompareTag("Player"))
            characterStatus = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            characterStatus = false;
    }

}
