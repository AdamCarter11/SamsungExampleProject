using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject obstaclePrefab;
    // Update is called once per frame
    private void Start() {
        StartCoroutine(SpawnObstacle());
    }
    void Update()
    {
        
    }
    private IEnumerator SpawnObstacle(){
        while(true){
            yield return new WaitForSeconds(2.0f);
            Instantiate(obstaclePrefab, new Vector2(Random.Range(-1.8f,1.8f), 5.5f) ,Quaternion.identity);
        }
    }
}
