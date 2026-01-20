using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Set AI state", story: "[Self] sets AI State through [animator]", category: "Action", id: "a2ce55c60dc71e6f4914eee0def0d580")]
public partial class SetAiStateAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<Animator> Animator;
    private int aiState;

    protected override Status OnUpdate()
    {
        Animator.Value.SetInteger("AIState", 2);
        return Status.Success;
    }
}

