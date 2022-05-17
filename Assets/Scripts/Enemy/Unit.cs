using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [Tooltip("Target for folowing (player)")]
    [SerializeField] public Transform _target;

    [Tooltip("Connection with Animated part")]
    [SerializeField] private ConfigurableJoint _joint;

    [Tooltip("Current transform")]
    [SerializeField] private Transform _pelvisTransform;

    private void FixedUpdate()
    {
        Vector3 toTarget = _target.position - _pelvisTransform.position;
        Vector3 toTargetXZ = new Vector3(toTarget.x, 0f, toTarget.z);
        Quaternion rotation = Quaternion.LookRotation(toTargetXZ);

        _joint.targetRotation = Quaternion.Inverse(rotation);
    }
}
