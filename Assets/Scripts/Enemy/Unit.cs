using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Unit : MonoBehaviour
{
    [Tooltip("Target for folowing (player)")]
    [SerializeField] public Transform _target;

    [Tooltip("Connection with Animated part")]
    [SerializeField] private ConfigurableJoint _joint;

    [Tooltip("Current transform")]
    [SerializeField] private Transform _pelvisTransform;

    private void Start() {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        Color color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);;
        foreach(Renderer renderer in renderers) {
            renderer.material.color = color;
        }
    }

    private void FixedUpdate()
    {
        Vector3 toTarget = _target.position - _pelvisTransform.position;
        Vector3 toTargetXZ = new Vector3(toTarget.x, 0f, toTarget.z);
        Quaternion rotation = Quaternion.LookRotation(toTargetXZ);

        _joint.targetRotation = Quaternion.Inverse(rotation);
    }
}
