using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Detect Player", story: "Check if [Target] is [Detected] change [CanSeePlayer] and log [LastknownPosition]", category: "Action", id: "f6fdc4f71c288ca864ac637fd8ed5172")]
public partial class DetectPlayerAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<DetectPlayer> Detected;
    [SerializeReference] public BlackboardVariable<bool> CanSeePlayer;

    protected override Status OnUpdate()
    {
        CanSeePlayer.Value = Detected.Value.CanSeeTarget();
        if (CanSeePlayer.Value == true)
        {
            CanSeePlayer.Value = true;
        } 
        return CanSeePlayer.Value ? Status.Success : Status.Failure;
    }
}

