using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketManager : EnemyManager
{
    public GameObject prefab = null;
    public List<float> heights = new List<float>();

    protected override float SpawnY{
        get {
            return heights[Random.Range(0, heights.Count)];
        }
    }

    protected override void createEnemy()
    {
        GameObject rocket = Instantiate(prefab, new Vector3(SpawnX, SpawnY, 0.0f), Quaternion.identity);
        existingEnemies.Add(rocket);
    }

    protected override void RunComponent()
    {
        base.RunComponent();
        foreach (GameObject rocket in existingEnemies)
        {
            rocket.GetComponent<Animator>().SetBool("GameRunning", true);
        }
    }

    protected override void StopComponent()
    {
        base.StopComponent();
        foreach (GameObject rocket in existingEnemies)
        {
            rocket.GetComponent<Animator>().SetBool("GameRunning", false);
        }
    }
}
