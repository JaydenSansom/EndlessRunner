using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelChunk : MonoBehaviour
{
    [System.Serializable]
    public struct RandomWeighting
    {
        [Tooltip("the level chunk with this random weighting to spawn next")]
        public GameObject nextChunk;
        [Tooltip("the randomized weight applied to this object. 0 means it will never appear, larger means more likely to appear")]
        public float weight;
    }

    [SerializeField, Tooltip("the speed at which this chunk is moving left")]
    private float moveSpeed = 1;
    [SerializeField, Tooltip("the holder object for this level, determines spawning positions")]
    private GameObject parentObj;
    [SerializeField, Tooltip("the position at which the next level should be spawned (should be on the far right end")]
    private GameObject nextAnchorObject;
    [SerializeField, Tooltip("A list of all possible next chunks to spawn and the likelihood of them spawning")]
    private List<RandomWeighting> nextChunks = new List<RandomWeighting>();

    private float maxWeights = 0;
    // Start is called before the first frame update
    void Start()
    {
        if(!parentObj){
            parentObj = transform.parent.gameObject;
        }
        foreach (RandomWeighting chunk in nextChunks)
        {
            maxWeights++;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        parentObj.transform.position += new Vector3(-moveSpeed * Time.fixedDeltaTime, 0, 0);
    }


    public GameObject GetNextChunk()
    {
        float rand = Random.Range(0, maxWeights);
        float valCheck = 0;
        foreach (RandomWeighting chunk in nextChunks)
        {
            valCheck += chunk.weight;
            if (rand < valCheck)
            {
                return chunk.nextChunk;
            }
        }
        return nextChunks[^1].nextChunk;
        
    }

    public GameObject GetParent()
    {
        return parentObj;
    }

    public void SpawnNextChunk()
    {
        GameObject nextChunk = GetNextChunk();
        nextChunk = Instantiate(nextChunk, nextAnchorObject.transform.position, Quaternion.identity);

    }
}
