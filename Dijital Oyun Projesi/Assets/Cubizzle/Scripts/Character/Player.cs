using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Color = System.Drawing.Color;

public class Player : MonoBehaviour
{
    public float moveTime;
    bool moveStatus;
    [SerializeField] private int health;
    [SerializeField] private Animator animator;
    
    private void Start()
    {
        moveStatus = false;
    }

    private void OnEnable()
    {
        Alien.AddDamage += HealthCheck;
    }

    private void OnDisable()
    {
        Alien.AddDamage -= HealthCheck;
    }

    private void Move(PlayerOrientation orientation)
    {
        moveStatus = true;
        SpinnedCube spinnedCube = SpinnedCubeCheck();
        MovementController(true);
        switch (orientation)
        {
            case PlayerOrientation.Forward:
                
                if (OrientationCheck(Vector3.forward))
                {
                    CharacterMove(Vector3.forward);
                    if (spinnedCube != null)
                        spinnedCube.RotateCube(CubeOrientation.Forward);
                }
                else
                {
                    moveStatus = false;
                    MovementController(false);
                }
                break;
            
            case PlayerOrientation.Back:
                
                if (OrientationCheck(Vector3.back)){
                    CharacterMove(Vector3.back);
                    if (spinnedCube != null)
                        spinnedCube.RotateCube(CubeOrientation.Back);
                }
                else
                {
                    moveStatus = false;
                    MovementController(false);
                }
                break;
            
            case PlayerOrientation.Left:

                if (OrientationCheck(Vector3.left))
                {
                    CharacterMove(Vector3.left);
                    if (spinnedCube != null)
                        spinnedCube.RotateCube(CubeOrientation.Left);
                }
                else
                {
                    moveStatus = false;
                    MovementController(false);
                }
                break;
            
            case PlayerOrientation.Right:

                if (OrientationCheck(Vector3.right))
                {
                    CharacterMove(Vector3.right);
                    if (spinnedCube != null)
                        spinnedCube.RotateCube(CubeOrientation.Right);
                }
                else
                {
                    moveStatus = false;
                    MovementController(false);
                }
                break;
        }
       
    }

    private void CharacterMove(Vector3 movementTarget)
    {
        CharacterRotate(movementTarget);
        Vector3 moveTo = transform.position + movementTarget * 8;
        this.transform.DOMove(moveTo, moveTime).SetEase(Ease.Linear).OnComplete(() => {
            moveStatus = false;
            MovementController(false);
            Alien.AlienStepAdd?.Invoke();
        });
    }

    private void CharacterRotate(Vector3 movementTarget)
    {

        float movementDir = (movementTarget == Vector3.forward) ? 0 :
            (movementTarget == Vector3.left) ? 270 :
            (movementTarget == Vector3.right) ? 90 : 180;
        this.transform.DORotate(new Vector3(0, movementDir, 0), .2f);
    }

    private void MovementController(bool status)
    {
        animator.SetBool("Walk", status);
    }


    private void Update()
    {
        #if UNITY_EDITOR
        if (moveStatus == false && CheckTerrain())
        {
            if(Input.GetKeyDown(KeyCode.W))
                Move(PlayerOrientation.Forward);
            if(Input.GetKeyDown(KeyCode.S))
                Move(PlayerOrientation.Back);
            if(Input.GetKeyDown(KeyCode.D))
                Move(PlayerOrientation.Right);
            if (Input.GetKeyDown(KeyCode.A)) 
                Move(PlayerOrientation.Left);
        }
        #endif
        if (Input.touchCount > 0 && moveStatus == false && CheckTerrain())
        {
            Touch finger = Input.GetTouch(0);

            if (finger.phase == TouchPhase.Moved)
            {
                if (finger.deltaPosition.y > 30)
                {
                    Move(PlayerOrientation.Forward);
                } 
                if (finger.deltaPosition.y < -30)
                {
                    Move(PlayerOrientation.Back);
                }
                if (finger.deltaPosition.x > 30)
                {
                    Move(PlayerOrientation.Right);
                }
                if (finger.deltaPosition.x < -30)
                {
                    Move(PlayerOrientation.Left);
                }
            }
        }


    }

    #region Checks
    bool CheckTerrain()
    {
        bool terrainStat = false;
        int layerMask6 = 1 << 6;
        int layerMask7 = 1 << 7;
    
        RaycastHit hit;
        if (Physics.Raycast(transform.position + new Vector3(0, 1, 0), Vector3.down, out hit, 8f, (layerMask6 | layerMask7)) && (hit.transform.tag == "movedTerrain" || hit.transform.tag == "spinnedTerrain"))
        {
            terrainStat = true;
        }
        return terrainStat;
    }
    
    bool OrientationCheck(Vector3 orientation)
    {
        int layerMask = 1 << 8;
        layerMask = ~layerMask;
        bool hitStatus = true;
        RaycastHit hit;
        if (Physics.Raycast(transform.position + new Vector3(0, 1, 0), orientation, out hit, 8f, layerMask) && hit.transform.tag == "moveBlockObj")
        {
            hitStatus = false;
            moveStatus = false;

        }

        return hitStatus;
    }

    private SpinnedCube SpinnedCubeCheck()
    {
        int layerMask = 1 << 6;
        SpinnedCube spinnedTerrain = null;
        RaycastHit hit;
        if (Physics.Raycast(transform.position + new Vector3(0, 1, 0), Vector3.down, out hit, 2f, layerMask) &&
            hit.transform.tag == "spinnedTerrain")
        {
            spinnedTerrain = hit.transform.GetComponent<SpinnedCube>();
        }

        return spinnedTerrain;
    }

    void HealthCheck()
    {
        if (health <= 0)
        {
            GameManager.Death?.Invoke();
        }
        else
        {
            health--;
        }
    }
    
    #endregion
    
    private void OnCollisionEnter(Collision other)
    {
        if(other.transform.tag == "lav")
           HealthCheck();
    }
}

public enum PlayerOrientation
{
    None,
    Forward,
    Back,
    Left,
    Right
}