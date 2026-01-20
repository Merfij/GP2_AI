using System;
using UnityEngine;
using Unity.Behavior;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(
    name: "AI State",
    story: "Set AI State",
    category: "Action",
    id: "f8e539f8206de5a130c412cb7316b8bd"
)]
public partial class AIStateAction : Unity.Behavior.Action
{
    public AIState state;

    private AIAnimatorController anim;

    protected override Status OnStart()
    {
        //if (anim == null)
        //    anim = Agent.GetComponent<AIAnimatorController>();

        anim.SetState(state);
        return Status.Success;
    }
}


