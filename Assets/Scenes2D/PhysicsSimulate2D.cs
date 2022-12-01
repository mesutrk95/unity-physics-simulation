using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PhysicsSimulate2D : MonoBehaviour
{
    public enum SimulateStatus
    {
        Success, Failed, TimeLimitFail, 
    }

    [System.Serializable]
    public class Player
    {
        public string x;
        public string y;
        public string id;
    }

    [System.Serializable]
    public class SimulateResult
    {
        public List<Player> players = new List<Player>();
        public string timeStep;
        public int iterations;
        public long calcTime;
        public string status;

    }

    public bool enableAutoSimulation = false; 
    public Transform objectsHolder;
    private List<Vector3> _initPoses = new List<Vector3>();
    private List<Quaternion> _initRots = new List<Quaternion>();

    public int iterations = 10;
    public float timeStep = 0.01f;

    public PhysicsScene physicsScene;
    public Objects2D objects;

    void Awake()
    { 
        Debug.Log("Unity Instance Started.");
        Physics.autoSimulation = false;
        physicsScene = gameObject.scene.GetPhysicsScene();

        Debug.Log("Scene = " + gameObject.scene.name);
        Debug.Log("Physics Scene = " + physicsScene.ToString());
            
        for (int i = 0; i < objectsHolder.childCount; i++)
        {
            _initPoses.Add(objectsHolder.GetChild(i).position ); 
            _initRots.Add(objectsHolder.GetChild(i).rotation );
        } 
    }

    [ContextMenu("AutoSimulation")]
    void EnableAutoSim()
    {
        Physics.autoSimulation = true;
    }

    //void FixedUpdate()
    //{
    //    if (enableAutoSimulation)
    //    {
    //        physicsScene.Simulate(Time.fixedDeltaTime);
    //    }
    //}
    
    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Debug.Log("simulation");
            SimulatePhysics();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("one step simulation ");
            physicsScene.Simulate(timeStep);
        }
        if (Input.GetMouseButtonDown(1))
        {
            objects.Hit(); 
            Debug.Log("froce ");
        }
    }


    [ContextMenu("Reset")]
    public void Reset() {
        for (int i = 0; i < objectsHolder.childCount; i++)
        {
            objectsHolder.GetChild(i).transform.position = _initPoses[i];
            objectsHolder.GetChild(i).transform.rotation = _initRots[i]; 
        } 
    }

    [ContextMenu("SimulatePhysics")]
    public void SimulatePhysics()
    {

        for (int i = 0; i < iterations; i++)
        {
            physicsScene.Simulate(timeStep);
        }
    }

    string v2s(Vector3 vec){
        return $"{vec.x.ToString(".000000")},{vec.y.ToString(".000000")},{vec.z.ToString(".000000")}" ;
    }
     
    async public Task<SimulateResult> SimulateToIdle()
    {
        var timer = new System.Diagnostics.Stopwatch(); 
        var maxCalcTime = 5000;  
        var _iterations = 0; 

        var isIdle = false;
        var lastPosAndRots = new List<string>(); 
 
        //init pos & rots
        for (int i = 0; i < objectsHolder.childCount; i++)
        {
            var s = v2s(objectsHolder.GetChild(i).position) + "-" + v2s(objectsHolder.GetChild(i).eulerAngles); 
            lastPosAndRots.Add(s);
        } 

        Debug.Log("calculation started");
        timer.Start();

        while (!isIdle)
        { 
            physicsScene.Simulate(timeStep);

            var idle = true;
            for (int i = 0; i < objectsHolder.childCount; i++)
            {
                var pos = objectsHolder.GetChild(i).position;
                var rot = objectsHolder.GetChild(i).eulerAngles;
                var currentPosRot = v2s(pos) + "-" + v2s(rot); 
                // Debug.Log(currentPosRot +"==="+ lastPosAndRots[i]);

                if(idle && currentPosRot != lastPosAndRots[i]){
                    idle = false; 
                }
                lastPosAndRots[i] = currentPosRot;
            } 

            isIdle = idle;
            _iterations++;
 
            if(timer.ElapsedMilliseconds > maxCalcTime){
                Debug.Log("calculation time limit");
                break;
            }

            // if physics take 10 seconds 
            if(_iterations % 10000 == 0)  
                await Task.Delay(500);
             
        }
        timer.Stop();

        Debug.Log("calculation finished : " + timer.ElapsedMilliseconds + "ms, iterations: "+ _iterations);

        var result = new SimulateResult();
        result.timeStep = timeStep.ToString();
        result.iterations = _iterations;
        result.calcTime = timer.ElapsedMilliseconds;
        result.status = (isIdle ? SimulateStatus.Success : SimulateStatus.TimeLimitFail).ToString();
        for (int i = 0; i < objectsHolder.childCount; i++)
        {
            result.players.Add(new Player() {
                x = objectsHolder.GetChild(i).position.x.ToString(), 
                y = objectsHolder.GetChild(i).position.y.ToString(), 
                id = i.ToString() 
            });
        }

        return result; 
    }
}