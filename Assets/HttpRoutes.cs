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
    }

    public List<Command> cmds = new List<Command>();
    public PhysicsSimulate simulator; 

    private void Start() {
    } 

    public void Update(){
        if (cmds.Count > 0)
        {
            var cmd = cmds[0];
            simulator.Reset();
            var result = simulator.SimulateToIdle();
            cmd.result = result;
            cmd.calculated = true;
            cmds.Remove(cmd);
        }
    }

    async public Task<PhysicsSimulate.SimulateResult> GetRequest(string method, Dictionary<string, object> nameParams)
    {
        Debug.Log("New GetRequest -> " + method);

        var cmd = new Command() { id = DateTime.UtcNow.Millisecond, calculated = false };
        cmds.Add(cmd);

        while (!cmd.calculated)
        {
            await Task.Delay(100); 
        }  

        return cmd.result;
    }
}

