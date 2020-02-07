using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeManager : EnemyManager
{
    public List<GameObject> spikePrefabs = new List<GameObject>();

    private float spikeSetDistance = 0.25f;

    protected override void createEnemy()
    {
        int spikeCount = Random.Range(1, 4);
        for (int i = 0; i < spikeCount; i++)
        {
            int spikeIndex = Random.Range(0, spikePrefabs.Count);
            float spawnX = SpawnX + (i * spikeSetDistance);
            GameObject spikeInstance = Instantiate(spikePrefabs[spikeIndex], Vector3.right * spawnX, Quaternion.identity);
            existingEnemies.Add(spikeInstance);
        }
    }
}
