using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Alien : MonoBehaviour
{
   public static Action AlienStepAdd;
   private int step;
   private bool active = false;
   [SerializeField] private Animator animator;
   [SerializeField] private int passiveTime, activeTime;
   public static Action AddDamage;
   private void OnEnable()
   {
      AlienStepAdd += AddStep;
   }

   private void OnDisable()
   {
      AlienStepAdd -= AddStep;
   }

   void AddStep()
   {
      step++;
      if (step == passiveTime)
      {
         this.transform.DOScaleY(0, .5f);
         this.transform.GetComponent<Collider>().enabled = false;
         active = false;
         step = 0;
      }

      if (step == activeTime)
      {
         this.transform.DOScaleY(1, .5f);
         this.transform.GetComponent<Collider>().enabled = true;
         active = true;
      }
   }

   void Attack()
   {
      if (active)
      {
         Jump();
         AddDamage?.Invoke();
      }
   }
   
   void Jump()
   {
      animator.SetTrigger("Jump");
   }

   private void OnTriggerEnter(Collider other)
   {
      if (other.CompareTag("Player"))
      {
         Attack();
      }
   }
}
