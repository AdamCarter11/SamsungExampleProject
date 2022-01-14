using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject obstaclePrefab;
    [SerializeField]
    private GameObject freezePrefab;
    private float[] whichSpot = {-2, -1.5f, -1, -.5f, 0, .5f, 1, 1.5f, 2};
    private float[] cubeSize = {.5f, 1, 1.5f};
    private int randoSize;
    [SerializeField]
    private float spawnRate;
    [SerializeField]
    private Camera cameraH;
    private float pastCameraY = 0f;
    private Color[] colors = {Color.red, Color.green, Color.yellow};
    private int powerUpSpawnRate = 5;
    private int amountObstSpawned = 0;
    private float spawnHeightOffset = 0;
    // Update is called once per frame
    private void Start() {
        StartCoroutine(SpawnObstacle());
    }
    void Update()
    {
        if(amountObstSpawned >= powerUpSpawnRate){
            powerUpSpawnRate = Random.Range(5,10);
            int randy = Random.Range(0,9);
            amountObstSpawned = 0;
            GameObject freezePowerup = Instantiate(freezePrefab, new Vector2(whichSpot[randy], 10 + pastCameraY), Quaternion.identity);
            if(freezePowerup.transform.position.y < pastCameraY-20f){
                Destroy(freezePowerup.gameObject);
            }
            if(cameraH.transform.position.y > pastCameraY){
                pastCameraY = cameraH.transform.position.y + 10;
            }
        }
    }
    private IEnumerator SpawnObstacle(){
        while(true){
            yield return new WaitForSeconds(spawnRate);
            amountObstSpawned++;
            int rando = Random.Range(0,9);
            //spawns cube at one of 9 random positions
            
            GameObject spawnedCube = Instantiate(obstaclePrefab, new Vector2(whichSpot[rando], 10 + pastCameraY + spawnHeightOffset) ,Quaternion.identity);
                spawnHeightOffset += .5f;
            //used to pick size and color
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

            //makes cubes spawn faster every time one is spawned to a limit
            spawnRate-=.01f;
            if(spawnRate<= .3f){
                spawnRate = .3f;
            }
        }
    }
}
