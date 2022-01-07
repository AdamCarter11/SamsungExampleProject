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
            int rando = Random.Range(0,9);
            float[] whichSpot = {-2, -1.5f, -1, -.5f, 0, .5f, 1, 1.5f, 2};
            Instantiate(obstaclePrefab, new Vector2(whichSpot[rando], 5.5f) ,Quaternion.identity);
        }
    }
}
