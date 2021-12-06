using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinnedCube : MonoBehaviour
{
    [SerializeField] SpinnedCubeValues spinnedCubeValues;
    float spinSpeed;
    bool spinCheck;
    private void Awake()
    {
        spinSpeed = spinnedCubeValues.spinSpeed;
    }

    public void RotateCube(CubeOrientation cubeOrientation)
    {
        Vector3 cubePosition = this.transform.position;
        
        if(!spinCheck)
        {
            switch (cubeOrientation)
            {
                case CubeOrientation.Forward:
                    if(OrientationCheck(Vector3.forward))
                        StartCoroutine(SpinCube(cubePosition + new Vector3(0, 4f, 4f), Vector3.left));
                    break;
                case CubeOrientation.Back:
                    if (OrientationCheck(Vector3.back))
                        StartCoroutine(SpinCube(cubePosition + new Vector3(0, 4f, -4f), Vector3.right));
                    break;
                case CubeOrientation.Left:
                    if(OrientationCheck(Vector3.left))
                        StartCoroutine(SpinCube(cubePosition + new Vector3(-4f, 4f, 0), Vector3.back));
                    break;
                case CubeOrientation.Right:
                    if(OrientationCheck(Vector3.right))
                        StartCoroutine(SpinCube(cubePosition + new Vector3(4f, 4f, 0), Vector3.forward));
                    break;
            }
        }
    }

    IEnumerator SpinCube(Vector3 pivotPoint, Vector3 orientationVector)
    {
        spinCheck = true;
        for (int i = 0; i < (90 / spinSpeed); i++)
        {
            this.transform.RotateAround(pivotPoint, orientationVector, spinSpeed);
            yield return new WaitForSeconds(0.001f);
        }
        Vector3 stabilPosition = new Vector3(ConvertToInteger((double)transform.position.x), ConvertToInteger((double)transform.position.y), ConvertToInteger((double)transform.position.z));
        this.transform.position = stabilPosition;
        spinCheck = false;
        yield return null;
    }

    bool OrientationCheck(Vector3 orientation)
    {
        int layerMask = 1 << 8;
        layerMask = ~layerMask;
        bool hitStatus = true;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, orientation, out hit, 8f, layerMask))
        {
            hitStatus = false;
           
        }
        return hitStatus;
    }


    int ConvertToInteger(double number)
    {
        return Mathf.RoundToInt((float)number);
    }

}

public enum CubeOrientation
{
    None,
    Forward,
    Back,
    Left,
    Right
}
