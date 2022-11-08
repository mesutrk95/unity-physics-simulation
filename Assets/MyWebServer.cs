using System.Collections;
using System.Collections.Generic;
using UnityEngine;  
using System;  
using System.Text; 
using System.Net;
using System.IO;
using System.Threading; 
using System.Threading.Tasks;

public class MyWebServer : MonoBehaviour
{ 
    static int bufferSize = 16;
    public HttpRoutes _methodController; 

    private Thread _serverThread; 
    private HttpListener _listener;
    public int port = 5980; 
     
    public void Start()
    {
        var port = Environment.GetEnvironmentVariable("PORT", EnvironmentVariableTarget.User);
        Debug.Log("PORT=" + port);
        if (!string.IsNullOrEmpty(port))
        {
            this.port = int.Parse( port );
        }

        _serverThread = new Thread(this.Listen);
        _serverThread.Start();
    }
     
      
    public void Stop()
    {
        _serverThread.Abort();
        _listener.Stop();
    }

    async private void Listen()
    {
        string url = "http://127.0.0.1:" + port.ToString() + "/";
        _listener = new HttpListener();
        _listener.Prefixes.Add(url);
        _listener.Start();

        Debug.Log("Web server listening at " + url);

        while (true)
        {
            try
            {
                HttpListenerContext context = _listener.GetContext();
                await Process(context);
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log(ex);
            }
        }
    }

    private Dictionary<string, object> _GetNamedParams(HttpListenerContext context)
    {
        Dictionary<string, object> namedParameters = new Dictionary<string, object>();
        if (!string.IsNullOrEmpty(context.Request.Url.Query))
        {
            UnityEngine.Debug.Log(context.Request.Url.Query);
            var query = context.Request.Url.Query.Replace("?", "").Split('&');
            foreach (var item in query)
            {
                var t = item.Split('=');


                namedParameters.Add(t[0], t[1]);
            }
        }

        return namedParameters;
    }


    async private Task Process(HttpListenerContext context)
    {
        string method = context.Request.Url.AbsolutePath;
        method = method.Substring(1);
        Debug.Log("New GetRequest -> " + method); 

        if (method == "Simulate")
        {
            var namedParams = _GetNamedParams(context);
            var result = await _methodController.SimulateRequest(namedParams);

            byte[] jsonByte = Encoding.UTF8.GetBytes(JsonUtility.ToJson(result));
            context.Response.ContentLength64 = jsonByte.Length;
            context.Response.ContentType = "application/json";
            Stream jsonStream = new MemoryStream(jsonByte);
            byte[] buffer = new byte[1024 * bufferSize];
            int nbytes;
            while ((nbytes = jsonStream.Read(buffer, 0, buffer.Length)) > 0)
                context.Response.OutputStream.Write(buffer, 0, nbytes);

            jsonStream.Close();
            context.Response.OutputStream.Flush();
            context.Response.OutputStream.Close();
        }
        else
        { 
            context.Response.OutputStream.Flush();
            context.Response.OutputStream.Close();
        }
    }

       
}
 