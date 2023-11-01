using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetLauncher : MonoBehaviour
{
    [Header("Line renderer")]
    [SerializeField, Tooltip("the line renderer for this object")] private LineRenderer lineRenderer;
    [SerializeField, Tooltip("if true, this line will draw all the way to the target regardless of distance, if false, it will use the max length defined below")] private bool drawFullLine = true;
    [SerializeField, Tooltip("the maximum length of the line to be drawn (if the above is marked true)")]private float maxLineLength;
    [Header("reticle")]
    [SerializeField, Tooltip("the object for the launcher's reticle")]private GameObject reticle;
    [SerializeField, Tooltip("the renderer for the reticle")]private SpriteRenderer reticleRenderer;
    [SerializeField,Tooltip("whether or not the reticle should be used")]private bool useReticle = true;
    [SerializeField, Tooltip("if true, the reticle will stick to the target, if false it will draw at the end of the line")]private bool reticleStickToTarget = true;
    [Header("new spawning")]
    [SerializeField, Tooltip("the transform this launcher will aim at")] private Transform NetTarget;
    [SerializeField, Tooltip("the delay before this launcher launches a net")] private float NetLaunchDelay = 3;
    [SerializeField, Tooltip("the portion of the launcher's delay where it can no longer alter its trajectory")]private float lockedInTime = .5f;
    private float launchTimer = 0;
    private bool timerCounting = false;
    [SerializeField, Tooltip("the net prefab to launch")] private GameObject NetPrefab;
    [SerializeField, Tooltip("the frequency of the flashing when preparing to launch the net. Higher numbers mean faster flashes")] private float A = 3;
    [Header("Debug")]
    [SerializeField, Tooltip("if marked true, will start firing countdown on game start")]private bool useDebug;
    [SerializeField, Tooltip("the target to use for debugging")]private Transform debugTarget;

    private Vector3 trajectory;

    private Gradient cg;
    

    // Start is called before the first frame update
    void Start()
    {
        if(!lineRenderer){
            lineRenderer = gameObject.GetComponent<LineRenderer>();
            if(!lineRenderer){
                lineRenderer = gameObject.AddComponent<LineRenderer>();
            }
        }
        if(useReticle && reticle && !reticleRenderer){
            reticleRenderer = reticle.GetComponent<SpriteRenderer>();
        }
        if(!useReticle && reticle){
            reticle.SetActive(false);
        }
        #if UNITY_EDITOR
        if(useDebug && debugTarget){
            StartDelay(debugTarget);
        }
        #endif
        cg = lineRenderer.colorGradient;
    }

    public void StartDelay(Transform newTarget, float delayTime = -1){
        if(delayTime != -1){
            NetLaunchDelay = delayTime;
        }
        NetTarget = newTarget;
        launchTimer = NetLaunchDelay;
        timerCounting = true;
        trajectory = NetTarget.position - transform.position;
        if (reticle)
        {
            reticle.SetActive(true);
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(!GameManager.Instance.isPlaying()){
            return;
        }
        if(timerCounting){
            float alpha = 255;
            launchTimer -= Time.fixedDeltaTime;
            if(launchTimer > lockedInTime){
                trajectory = NetTarget.position - transform.position;
                Vector3 endPoint = NetTarget.position;
                if(!drawFullLine){
                    if(trajectory.magnitude > maxLineLength){
                        endPoint = transform.position + (trajectory.normalized * maxLineLength);
                    }
                }
                lineRenderer.SetPositions(new Vector3[] { transform.position, endPoint });
            

                if(useReticle){
                    if(reticleStickToTarget){
                        reticle.transform.position = NetTarget.position;
                    }
                    else{
                        reticle.transform.position = endPoint;
                    }
                }
                float timeElapsed = NetLaunchDelay - launchTimer;
                alpha = -Mathf.Cos(A * Mathf.Pow(timeElapsed,2)) + 1;
                // Color newColor = new Color(lineRenderer.startColor.r, lineRenderer.startColor.g, lineRenderer.startColor.b, alpha);
                // lineRenderer.SetColors(newColor,newColor);
                // if(useReticle){
                //     reticleRenderer.color = newColor;
                // }
            }
            if(launchTimer < 0){
                FireNet();
            }

            Color newColor = new Color(lineRenderer.startColor.r, lineRenderer.startColor.g, lineRenderer.startColor.b, alpha);


            // Blend alpha from opaque at 0% to transparent at 100%
            var alphas = new GradientAlphaKey[2];
            alphas[0] = new GradientAlphaKey(alpha, 0.0f);
            alphas[1] = new GradientAlphaKey(alpha, 1.0f);

            cg.SetKeys(cg.colorKeys, alphas);
            
            lineRenderer.colorGradient = cg;
            
            //lineRenderer.materials[0].SetColor("_TintColor", newColor);
            if(useReticle){
                reticleRenderer.color = newColor;
            }
        }
    }

    public void FireNet(){
        GameObject launchedNet = Instantiate(NetPrefab, transform.position, Quaternion.identity, Camera.main.transform);
        launchedNet.GetComponent<Net>().SetHeading(trajectory);
        Destroy(gameObject);
    }
}
