using UnityEngine;

public class LightSphere : MonoBehaviour
{
    public SphereCollider sphere;
    public MeshRenderer sphereMesh;
    public void RemoveSphere()
    {
        sphere.enabled = false;
        sphereMesh.enabled = false;
    }
}
