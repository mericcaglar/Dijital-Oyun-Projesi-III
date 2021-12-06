using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MovingCube : MonoBehaviour
{
    bool characterStatus, cubeStatus;
    GameObject player;
    [SerializeField] private Material[] materials;

    private void OnMouseDown()
    {
        if (characterStatus)
            if (cubeStatus)
                PutCube();
            else
                GetCube();
        
    }

    void PutCube()
    {
        this.transform.parent = null;
        cubeStatus = false;
    }

    void GetCube()
    {
        this.transform.parent = player.transform;
        cubeStatus = true;
    }

    void LevelEnd(Transform transform)
    {
        this.transform.parent = transform.parent;
        this.transform.DOMove(transform.parent.transform.position, 1f).OnComplete(()=> GameManager.LevelEnd?.Invoke());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            characterStatus = true;
        }

        if (other.CompareTag("rover"))
        {
            LevelEnd(other.transform);

        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            characterStatus = false;
            player = null;
        }
           
    }
}
