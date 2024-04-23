using UnityEngine;

namespace Abilities
{
    public class MissileSpawner : MonoBehaviour
    {
        public GameObject prefabToSpawn;
        public int numberOfObjectsToSpawn;
        public float spawnAreaRadius;
        public LayerMask playerMask;

        public void Spawn()
        {
            for (int i = 0; i < numberOfObjectsToSpawn; i++)
            {
                Vector3 randomPosition = GetRandomPosition();
                Collider[] playerColliders = Physics.OverlapSphere(randomPosition, 0.5f, playerMask);

                if (playerColliders.Length == 0)
                {
                    Instantiate(prefabToSpawn, randomPosition, Quaternion.identity);
                }
            }
        }

        Vector3 GetRandomPosition()
        {
            Vector3 randomDirection = Random.insideUnitSphere * spawnAreaRadius;
            randomDirection += transform.position;
            randomDirection.y = 0;
            return randomDirection;
        }
    }
}