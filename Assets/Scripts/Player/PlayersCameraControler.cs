using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayersCameraControler : MonoBehaviour
{
    [SerializeField]
    private GameObject FollowTarget;
    [SerializeField]
    private Camera Camera;
    [SerializeField]
    private Vector3 Offset;

    private void Update()
    {
        Camera.transform.position = FollowTarget.transform.position + Offset;
    }
}
