using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelChunk : MonoBehaviour
{
    [System.Serializable]
    public struct RandomWeighting
    {
        [Tooltip("the object that will spawn if this is selected")]
        public GameObject randObject;
        [Tooltip("the randomized weight applied to this object. 0 means it will never appear, larger means more likely to appear")]
        public float weight;
    }

    [System.Serializable]
    public struct RandomPiecesSpawnChunk{
        [Tooltip("the number of objects that will be selected from this list to spawn")]
        public int numToSpawn;
        [Tooltip("all of the random options to pick from")]
        public List<RandomWeighting> randomOptions;
    }

    [SerializeField, Tooltip("the holder object for this level, determines spawning positions")]
    private GameObject parentObj;
    [SerializeField, Tooltip("the position at which the next level should be spawned (should be on the far right end")]
    private GameObject nextAnchorObject;
    [SerializeField, Tooltip("A list of all possible next chunks to spawn and the likelihood of them spawning")]
    private List<RandomWeighting> nextChunks = new List<RandomWeighting>();
    [SerializeField, Tooltip("A list of all of the randomly spawned pieces within this level section")]
    private List<RandomPiecesSpawnChunk> randPieces = new List<RandomPiecesSpawnChunk>();
    [SerializeField, Tooltip("the tag for the killzone, used to despawn object")]
    private string killzoneTag = "KillZone";
    [SerializeField, Tooltip("the tag for the spawn area, used to despawn object")]
    private string spawnzoneTag = "SpawnZone";
    [SerializeField]
    private bool hasSpawnedNextArea = false;
    [SerializeField, Tooltip("the max distance this object can be from the camera before it destroys itself")]
    private float maxSafetyDist = 25;

    [Header("debug/vid stuff")]
    [SerializeField, Tooltip("the velocity to move this at")]
    private float moveSpeed = 1;

    private float maxWeights = 0;
    [SerializeField, Tooltip("if marked true, will manually spawn segments for testing")]
    private bool debugTestSpawn = false;
    // Start is called before the first frame update
    void Start()
    {
        if(!parentObj){
            parentObj = transform.parent.gameObject;
        }
        SpawnRandObjects();
        if(debugTestSpawn){
            Invoke("SpawnNextChunk", 0.5f);
            Debug.Log("Spawning chunk through invoke");
        }
    }

    private void FixedUpdate() {
        Vector3 cameraPos = Camera.main.transform.position;
        if(cameraPos.x > transform.position.x && (cameraPos - transform.position).magnitude > maxSafetyDist){
            Destroy(gameObject);
        }
        parentObj.transform.position += new Vector3(-moveSpeed,0,0);
    }


    public void SpawnRandObjects(){
        foreach(RandomPiecesSpawnChunk randChunk in randPieces){
            if(randChunk.randomOptions.Count > 0){
                GetRandomObjects(randChunk.randomOptions, randChunk.numToSpawn, true);
            }
        }
    }

    public RandomWeighting GetRandomObject(List<RandomWeighting> randOptions)
    {
        if(randOptions.Count < 0){
            Debug.LogError("RANDOMOBJECT");
            return new RandomWeighting();
        }
        //Debug.Log("rand " + randOptions);
        foreach (RandomWeighting option in randOptions)
        {
            maxWeights+= option.weight;
        }
        float rand = Random.Range(0, maxWeights);
        float valCheck = 0;
        foreach (RandomWeighting option in randOptions)
        {
            valCheck += option.weight;
            if (rand < valCheck)
            {
                return option;
            }
        }
        return randOptions[^1];
        
    }

    public List<GameObject> GetRandomObjects(List<RandomWeighting> randOptions, int numToGet, bool enableObjects){
        List<RandomWeighting> options = new List<RandomWeighting>(randOptions);
        if(numToGet > randOptions.Count){
            List<GameObject> gameObjs = new List<GameObject>();
            foreach(RandomWeighting obj in options){
                gameObjs.Add(obj.randObject);
            }
            return gameObjs;
        }
        if(numToGet < 1){
            return null;
        }
        List<GameObject> selectedObjects = new List<GameObject>();
        for(int i = 0; i < numToGet; i++){
            RandomWeighting selectedObject = GetRandomObject(options);
            selectedObjects.Add(selectedObject.randObject);
            options.Remove(selectedObject);
            if(enableObjects){
                selectedObject.randObject.SetActive(true);
            }
        }
        if(enableObjects){
            foreach(RandomWeighting obj in options){
                obj.randObject.SetActive(false);
            }
        }
        return selectedObjects;
    }

    public GameObject GetParent()
    {
        return parentObj;
    }

    public void SpawnNextChunk()
    {
        if(hasSpawnedNextArea){
            return;
        }
        GameObject nextChunk = GetRandomObject(nextChunks).randObject;
        nextChunk = Instantiate(nextChunk, nextAnchorObject.transform.position, Quaternion.identity);
        hasSpawnedNextArea = true;
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(!this.gameObject.scene.isLoaded) return;
        if(other.gameObject.tag == killzoneTag){
            Destroy(parentObj);
        }
        if(!hasSpawnedNextArea && other.gameObject.tag == spawnzoneTag){
            SpawnNextChunk();
        }
    }
    
}
