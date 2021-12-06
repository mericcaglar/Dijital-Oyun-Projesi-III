using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   public static Action LevelEnd;
   public static Action Death;

   [SerializeField]private GameObject[] levels;
   [SerializeField]private Transform parentObj;
   private int activeLevel;

   private void OnEnable()
   {
      LevelEnd += LevelComplete;
      Death += GameOver;
   }

   private void OnDisable()
   {
      LevelEnd -= LevelComplete;
      Death -= GameOver;
   }

   private void Awake()
   {
      LoadLevel(0);
   }

   void LoadLevel(int level)
   {
      activeLevel = level;
      Instantiate(levels[level], Vector3.zero, Quaternion.identity,parentObj);
   }
   
   void LevelComplete()
   {
      
   }

   void GameOver()
   {
      Debug.Log("Oyun Bitti");
   }
}
