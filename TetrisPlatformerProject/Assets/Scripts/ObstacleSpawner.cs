using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject obstaclePrefab;
    private float[] whichSpot = {-2, -1.5f, -1, -.5f, 0, .5f, 1, 1.5f, 2};
    private float[] cubeSize = {.5f, 1, 1.5f};
    private int randoSize;
    [SerializeField]
    private float spawnRate;
    [SerializeField]
    private Camera cameraH;

    private Color[] colors = {Color.red, Color.green, Color.yellow};
    // Update is called once per frame
    private void Start() {
        StartCoroutine(SpawnObstacle());
    }
    void Update()
    {
        
    }
    private IEnumerator SpawnObstacle(){
        while(true){
            yield return new WaitForSeconds(spawnRate);
            int rando = Random.Range(0,9);
            GameObject spawnedCube = Instantiate(obstaclePrefab, new Vector2(whichSpot[rando], 6f + cameraH.transform.position.y) ,Quaternion.identity);

            if(rando == 0 || rando == 8){
                randoSize = 0;
            }
            else if (rando == 1 || rando == 7){
                randoSize = Random.Range(0,2);
            }
            else{
                randoSize = Random.Range(0,3);
            }
            spawnedCube.GetComponent<SpriteRenderer>().color = colors[randoSize];
            spawnedCube.transform.localScale = new Vector2(cubeSize[randoSize], cubeSize[randoSize]);
        }
    }
}
