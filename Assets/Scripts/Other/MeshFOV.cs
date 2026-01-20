using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MeshFOV : MonoBehaviour
{
    [Header("FOV Settings")]
    [SerializeField] private float viewRadius = 10f;
    [SerializeField, Range(0, 360)] private float viewAngle = 90f;
    [SerializeField] private int meshResolution = 50;

    [Header("Obstacle Detection")]
    [SerializeField] private LayerMask obstacleMask;

    [Header("Origin")]
    [SerializeField] private Transform eyePosition;

    private Mesh mesh;

    private void Awake()
    {
        mesh = new Mesh();
        mesh.name = "FOV Mesh";
        GetComponent<MeshFilter>().mesh = mesh;
    }

    private void LateUpdate()
    {
        DrawFOV();
    }

    private void DrawFOV()
    {
        Transform eyes = eyePosition != null ? eyePosition : transform;

        int vertexCount = meshResolution + 2;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;

        float angleStep = viewAngle / meshResolution;
        float currentAngle = -viewAngle / 2f;

        for (int i = 0; i <= meshResolution; i++)
        {
            Vector3 dir = DirFromAngle(currentAngle);
            Vector3 vertex;

            if (Physics.Raycast(eyes.position, eyes.TransformDirection(dir),
                out RaycastHit hit, viewRadius, obstacleMask))
            {
                vertex = transform.InverseTransformPoint(hit.point);
            }
            else
            {
                vertex = dir * viewRadius;
            }

            vertices[i + 1] = vertex;

            if (i < meshResolution)
            {
                int index = i * 3;
                triangles[index] = 0;
                triangles[index + 1] = i + 1;
                triangles[index + 2] = i + 2;
            }

            currentAngle += angleStep;
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    private Vector3 DirFromAngle(float angle)
    {
        return Quaternion.Euler(0, angle, 0) * Vector3.forward;
    }

    // Optional: Live update in editor
    private void OnValidate()
    {
        if (mesh != null)
            DrawFOV();
    }
}

