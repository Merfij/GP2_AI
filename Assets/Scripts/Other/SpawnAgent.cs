using UnityEngine;

    public class AgentSpawner : MonoBehaviour
    {
        public EnemyFSM_NoPatrol EnemyPrefab;
        public int agentCount = 100;
        public Vector2 spawnAreaSize = new Vector2(10f, 10f);

        [Header("Target Assignment")]
        [Tooltip("Assign a target Transform that all spawned agents will seek/arrive at.")]
        public Transform sharedTarget;

        void Start()
        {
            for (int i = 0; i < agentCount; i++)
            {
                Vector3 offset = new Vector3(
                    Random.Range(-spawnAreaSize.x * 0.5f, spawnAreaSize.x * 0.5f),
                    0f,
                    Random.Range(-spawnAreaSize.y * 0.5f, spawnAreaSize.y * 0.5f)
                );

                Vector3 spawnPos = transform.position + offset;
                EnemyFSM_NoPatrol newAgent = Instantiate(EnemyPrefab, spawnPos, Quaternion.identity);

                // Assign shared target if specified
                if (sharedTarget != null)
                {
                    newAgent.target = sharedTarget;
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(transform.position, new Vector3(spawnAreaSize.x, 0.1f, spawnAreaSize.y));
        }
    }