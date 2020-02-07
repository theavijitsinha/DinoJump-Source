using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyManager : Runnable
{
    public GameManager gameManager = null;
    public float baseVelocity = 0.0f;
    public float intervalMin = 0;
    public float intervalMax = 0;

    protected float SpawnX { get => 8.0f; }
    protected virtual float SpawnY { get => 0.0f; }
    private float DestructX { get => -SpawnX; }
    private float Velocity { get => baseVelocity * gameManager.GameSpeedMultiplier; }

    protected List<GameObject> existingEnemies = new List<GameObject>();
    private Coroutine createEnemyCoroutine = null;

    protected override void RunComponent()
    {
        foreach (GameObject enemy in existingEnemies)
        {
            Destroy(enemy);
        }
        existingEnemies.Clear();
        createEnemyCoroutine = StartCoroutine(createEnemyLoop());
    }

    protected override void StopComponent()
    {
        StopCoroutine(createEnemyCoroutine);
    }

    private void FixedUpdate()
    {
        if (GameRunning)
        {
            moveEnemy(Velocity * Time.fixedDeltaTime);
            clearOutOfBoundsEnemies();
        }
    }

    private IEnumerator createEnemyLoop()
    {
        while (true)
        {
            float spikeInterval = Random.Range(intervalMin, intervalMax);
            yield return new WaitForSeconds(spikeInterval);
            createEnemy();
        }
    }

    protected abstract void createEnemy();

    private void moveEnemy(float displacement)
    {
        foreach (GameObject enemy in existingEnemies)
        {
            Rigidbody2D rb2D = enemy.GetComponent<Rigidbody2D>();
            rb2D.MovePosition(rb2D.position + (Vector2.right * displacement));
        }
    }

    private void clearOutOfBoundsEnemies()
    {
        List<GameObject> enemiesToDelete = new List<GameObject>();
        foreach (GameObject enemy in existingEnemies)
        {
            if (enemy.transform.position.x < DestructX)
            {
                enemiesToDelete.Add(enemy);
            }
        }
        foreach (GameObject enemy in enemiesToDelete)
        {
            existingEnemies.Remove(enemy);
            Destroy(enemy);
        }
        enemiesToDelete.Clear();
    }
}
