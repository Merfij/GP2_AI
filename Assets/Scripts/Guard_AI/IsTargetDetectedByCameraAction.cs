using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Is Target detected by Camera", story: "Is [Target] detected by [Camera]", category: "Action", id: "7ce86d14b1ca8509dd4cff20c15ce6ec")]
public partial class IsTargetDetectedByCameraAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<CameraEyes> Camera;
    [SerializeReference] public BlackboardVariable<bool> SightedByCamera;


    protected override Status OnUpdate()
    {
        if (Camera.Value.isPlayerInSight) { 
            SightedByCamera.Value = true;
        } else
        {
            SightedByCamera.Value = false;
        }
        Debug.Log($"IsTargetDetectedByCameraAction: Target detected by Camera = {SightedByCamera.Value}");
        return SightedByCamera.Value ? Status.Success : Status.Failure;
    }

}

