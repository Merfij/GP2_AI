using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Is Target detected by Camera", story: "Is [Target] Detected By [Camera]", category: "Action", id: "b63136eb70b92d0a01da17ebfeedad0a")]
public partial class IsTargetDetectedByCameraAction_2 : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<CameraEyes> Camera;
    [SerializeReference] public BlackboardVariable<bool> SightedByCamera;

    protected override Status OnUpdate()
    {
        if (Camera.Value == null)
        {
            Debug.LogWarning("IsTargetDetectedByCameraAction_2: Camera is null");
            return Status.Failure;
        }
        if (Camera.Value.isPlayerInSight == true)
        {
            SightedByCamera.Value = true;
        }
        return SightedByCamera.Value ? Status.Success : Status.Failure;
    }

}

