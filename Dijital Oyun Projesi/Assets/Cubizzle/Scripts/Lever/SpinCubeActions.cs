using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinCubeActions : MonoBehaviour
{
    [SerializeField] private int leverId;
    [SerializeField] private Renderer renderer;
    [SerializeField] private Material[] materials;
    private void OnEnable()
    {
        Lever.leverObjectOnActivate += CubeActive;
        Lever.leverObjectOnDisable += CubeDisable;
    }

    private void OnDisable()
    {
        Lever.leverObjectOnActivate -= CubeActive;
        Lever.leverObjectOnDisable -= CubeDisable;
    }


    void CubeActive(int id)
    {
        if(leverId == id)
        {
            GetComponent<Collider>().enabled = true;
            renderer.material = materials[0];
        }
    }

    void CubeDisable(int id)
    {
        if(leverId == id)
        {
            GetComponent<Collider>().enabled = false;
            renderer.material = materials[1];

        }
    }
}
