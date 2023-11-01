using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetManager : MonoBehaviour
{
    [Header("Transforms")]
    [SerializeField, Tooltip("the player to use as the target for the net launchers")]
    private Transform player;
    [SerializeField, Tooltip("the lower bound of locations where the net launcher can be spawned")]
    private Transform netLauncherLowerBound;
    [SerializeField, Tooltip("the upper bound of locations where the net launcher can be spawned")]
    private Transform netLauncherUpperBound;
    [SerializeField]
    private Transform launcherParent;

    [Header("Spawn Timing")]
    [SerializeField, Tooltip("the initial delay before the first net launcher can spawn")]
    private float initialDelay = 10;
    private float initialTimer;
    private bool isRunning = false;
    [SerializeField, Tooltip("the base time for net launcher spawn delays. this is the shortest possible spawn delay")]
    private float netLauncherSpawnDelayMin = 15;
    [SerializeField, Tooltip("the absolute lowest that the launcher spawn min can be set to")]
    private float absoluteMinimum = 5;
    [SerializeField, Tooltip("the maximum net launcher spawn delay can be found by multiplying the min delay by this number")]
    private float netLauncherMaxMult = 3;
    private float spawnTimer;
    [SerializeField, Tooltip("the shooting delay to pass into the net launchers. if set to -1, they will use their default timers")]
    private float netLauncherShootDelay = -1;

    [Header("multishot")] 
    [SerializeField, Tooltip("the likelihood of any launch to be a multishot")]
    private float multishotChance = .1f;
    [SerializeField, Tooltip("the minimum number of shots in a multishot")] 
    private int multishotMin = 3;
    [SerializeField, Tooltip("the maximum number of shots in a multishot")]
    private int multishotMax = 6;
    [SerializeField, Tooltip("the percentage of the normal minimum launcher spawn delay used between multishots")]
    private float multishotDelayPercent = .2f;
    private float multishotTimer;
    private int multiShotCount;

    [Header("prefab")]
    [SerializeField, Tooltip("the prefab of the net launcher")]private GameObject netLauncherPrefab;
    // Start is called before the first frame update
    void Start()
    {
        initialTimer = initialDelay;
        isRunning = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (initialTimer > 0)
        {
            //Debug.Log(initialTimer);
            initialTimer -=Time.fixedDeltaTime;
            return;
        }
        else if (!isRunning)
        {
            ResetSpawnTimer();
            isRunning = true;
            return;
        }

        if (multiShotCount > 0)
        {
            multishotTimer -= Time.fixedDeltaTime;
            if (multishotTimer <= 0)
            {
                SpawnLauncher(true);
                ResetMultiSpawnTimer();
            }
        }

        spawnTimer -= Time.fixedDeltaTime;
        if (spawnTimer <= 0)
        {
            SpawnLauncher();
            ResetSpawnTimer();
        }
        
    }

    private void SpawnLauncher(bool partOfMultishot = false)
    {
        float mult = Random.Range(0.0f, 1.0f);
        Vector3 spawnPoint = netLauncherLowerBound.position + (netLauncherUpperBound.position - netLauncherLowerBound.position) * mult;
        GameObject launcher = Instantiate(netLauncherPrefab, spawnPoint, Quaternion.identity, launcherParent);
        launcher.GetComponent<NetLauncher>().StartDelay(player, netLauncherShootDelay);
        if (partOfMultishot)
        {
            multiShotCount--;
            return;
        }
        float multishotRNG = Random.Range(0.0f, 1.0f);
        if (multishotRNG < this.multishotChance)
        {
            Debug.Log("MULTISHOT");
            multiShotCount = Random.Range(multishotMin - 1, multishotMax);
            ResetMultiSpawnTimer();
        }
    }

    private void ResetMultiSpawnTimer()
    {
        multishotTimer = netLauncherSpawnDelayMin * multishotDelayPercent;
    }

    private void ResetSpawnTimer(bool multishot = false)
    {
        float min = 1.0f;
        if (multishot)
        {
            min = 2.0f;
        }
        float mult = Random.Range(min, netLauncherMaxMult);
        spawnTimer = netLauncherSpawnDelayMin * mult;
    }

    private void SetNewMinimum(float newMin)
    {
        netLauncherSpawnDelayMin = Mathf.Max(absoluteMinimum, newMin);
    }
}
