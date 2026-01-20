using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Assing Camera Eyes", story: "Is [CameraEyes] Assigned", category: "Action", id: "f7e43ee9d4781b81277fb61efeb4a519")]
public partial class AssingCameraEyesAction : Action
{
    [SerializeReference] public BlackboardVariable<CameraEyes> CameraEyes;
    [SerializeReference] public BlackboardVariable<GameObject> CameraGO;

    protected override Status OnStart()
    {
        if (CameraEyes.Value == null)
        {
            CameraEyes.Value = CameraGO.Value.GetComponent<CameraEyes>();
        }
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (CameraEyes.Value == null)
        {
            Debug.LogWarning("AssingCameraEyesAction: CameraEyes is null");
        }
        return CameraEyes.Value != null ? Status.Success : Status.Failure;
    }
}

