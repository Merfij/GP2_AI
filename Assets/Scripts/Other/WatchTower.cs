using UnityEngine;

public class WatchTower : MonoBehaviour
{
    [SerializeField] private CameraEyes cameraEyes;

    // Update is called once per frame
    void Update()
    {
        if (cameraEyes.isPlayerInSight)
        {
            transform.Rotate(Vector3.up * 0f * Time.deltaTime);
        } else
        {
            transform.Rotate(Vector3.up * 10f * Time.deltaTime);
        }
    }
}
