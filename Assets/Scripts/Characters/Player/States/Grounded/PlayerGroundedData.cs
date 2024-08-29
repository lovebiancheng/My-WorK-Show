using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerGroundedData 
{
    [field:SerializeField][field:Range(0f,25f)]public float baseSpeed { get; private set; }
    [field:SerializeField] public PlayerRotationData baseRotationData { get; private set; }
    [field: SerializeField] public PlayerWalkData walkData { get; private set; }
    [field: SerializeField] public PlayerRunData runData { get; private set; }
}
