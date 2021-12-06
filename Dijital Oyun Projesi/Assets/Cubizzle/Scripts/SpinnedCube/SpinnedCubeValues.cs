using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spinned Cube Value", menuName = "Spinned Cube/Spinned Cube Value")]
public class SpinnedCubeValues : ScriptableObject
{
    [Range(0,10)]
    public float spinSpeed;
}
