 using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using System;
using System.Net;
using System.IO;
using System.Threading.Tasks;

public class HttpRoutes : MonoBehaviour
{   

    public class Command
    {
        public int id;
        public PhysicsSimulate.SimulateResult result;
        public string cmd;
        public bool calculated = false;
        public bool cancelled = false; 
    }

    public List<Command> cmds = new List<Command>();
    public PhysicsSimulate simulator; 

    private void Start() {
    } 

    async public void Update(){
        if (cmds.Count > 0)
        {
            var cmd = cmds[0];
            simulator.Reset();
            var result = await simulator.SimulateToIdle();
            cmd.result = result;
            cmd.calculated = true;
            cmds.Remove(cmd);
        }
    }


    int getUtcNow()
    {
        System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        int cur_time = (int)(System.DateTime.UtcNow - epochStart).TotalMilliseconds;
        return cur_time;
    }

    async public Task<PhysicsSimulate.SimulateResult> GetRequest(string method, Dictionary<string, object> nameParams)
    {
        Debug.Log("New GetRequest -> " + method);

        var timer = new System.Diagnostics.Stopwatch();
        timer.Start();

        var cmd = new Command() { id = getUtcNow(), calculated = false, cancelled = false };
        cmds.Add(cmd);

        while (!cmd.calculated)
        {
            await Task.Delay(100);

            //if (timer.ElapsedMilliseconds > 10 * 1000)
            //{
            //    cmd.cancelled = true;
            //}
        }  

        return cmd.result;
    }
}

