using UnityEngine;
using System.Collections;

public class enemyRespawner : MonoBehaviour {

    public GameObject enemy1;
    public float respawnTime;
    public bool respawned;
    public float timePassed;
    public float minDistanceFromPlayer;
    public GameObject player;
    protected ArrayList Enemies;
    public int maxEnemies;
    [Tooltip("Area para nascimento dos inimigos")]
    public float rangeRadius;
    public int timesCleared;
    public bool isMonitor;
    public GameObject finishPoint;
    public bool canRespawn;
    public bool loot;

	// Use this for initialization
	void Start () {

        player = GameObject.FindGameObjectWithTag("Hero");
        Enemies = new ArrayList();
        for(int i = 0; i < maxEnemies; i++)
        {
            GameObject enemyObj = Instantiate(enemy1, transform.position + new Vector3(Random.Range(-rangeRadius, rangeRadius), 0.0f, Random.Range(rangeRadius, rangeRadius)), Quaternion.identity) as GameObject;
            Enemies.Add(enemyObj);
            enemyObj.GetComponent<ZombieBehavior>().DefineSpawnPoint(this.gameObject);
            enemyObj.GetComponent<ZombieBehavior>().Initialize(loot);
        }
        
        timePassed = 0.0f;
        timesCleared = 0;
    }

    
    // Update is called once per frame
    void Update () {

        timePassed += Time.deltaTime;
        if(timePassed >= respawnTime && canRespawn)
        {
            
            
            float distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);
            if(distanceFromPlayer > minDistanceFromPlayer && Enemies.Count < maxEnemies)
            {
                
                GameObject enemyObj = Instantiate(enemy1, transform.position + new Vector3(Random.Range(-1.5f, 1.5f), 0.0f, Random.Range(-1.5f, 1.5f)), Quaternion.identity) as GameObject;
                Enemies.Add(enemyObj);
                enemyObj.GetComponent<ZombieBehavior>().DefineSpawnPoint(this.gameObject);
                enemyObj.GetComponent<ZombieBehavior>().Initialize(loot);
                timePassed = 0.0f;
                if (isMonitor)
                {
                    finishPoint.GetComponent<FinishStagePoint>().HideFinishPoint();
                }
            }
        }
        
	}

    public void RemoveFromList(GameObject enemy)
    {
        Enemies.Remove(enemy);
        if (Enemies.Count == 0)
        {
            if(isMonitor)
            {
                finishPoint.GetComponent<FinishStagePoint>().ShowFinishPoint();
            }
            timesCleared++;
            respawnTime *= timesCleared;
        }
    }

    public void DefineFinishPoint(GameObject obj)
    {
        finishPoint = obj;
        isMonitor = true;
    }
}
