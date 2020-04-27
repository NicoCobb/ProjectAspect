using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;

public class UnityHttpListener : MonoBehaviour
{
    private HttpListener listener;
    private Thread listenerThread;
    static public LinkedList<Direction> TwitchMoves = new LinkedList<Direction>();

    void Start() {
        listener = new HttpListener();

        if (!HttpListener.IsSupported) {
            throw new NotSupportedException("The Http Server cannot run on this operating system.");
        }

        listener.Prefixes.Add("http://127.0.0.1:4000/");
        listener.Prefixes.Add("http://192.168.1.83:4000/");
        listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
        listener.Start();

        listenerThread = new Thread(startListener);
        listenerThread.Start();
    }

    void Update() {}

    private void startListener() {
        while (true) {               
            IAsyncResult result = listener.BeginGetContext(ListenerCallback, listener);
            result.AsyncWaitHandle.WaitOne();   
        }
    }

    private void ListenerCallback(IAsyncResult result) {
        HttpListenerContext context = listener.EndGetContext(result);
        
        // TODO 
        // string method_name = context.Request.Url.Segments[1].Replace("/", "");
        // string[] str_params = context.Request.Url.Segments.Skip(2).Select(s=>s.Replace("/","")).ToArray();

        // var method = this.GetType().GetMethod(method_name);
        // object[] method_params = method.GetParameters()
        //                         .Select((p, i) => Convert.ChangeType(str_params[i], p.ParameterType)).ToArray();

        // object ret = method.Invoke(this, method_params);
        // string retstr = JsonConvert.SerializeObject(ret);

        if (context.Request.HttpMethod == "POST") {
            string str_stream = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding).ReadToEnd();
            Dictionary<string, string[]> data = new Dictionary<string, string[]>();
            string[] data_pairs = str_stream.Split('&');
            foreach(string pair in data_pairs) {
                string[] tuple = pair.Split('=');
                data.Add(tuple[0], tuple[1].Split('+'));
            };
            RecieveMoves(data);
        }

        context.Response.Close();
    }

    private void RecieveMoves(Dictionary<string, string[]> data) {
        // @nico you can get a userid from data["userID"][0] like this:
        Debug.Log("uid: [" + data["userID"][0] + "]");

        TwitchMoves = new LinkedList<Direction>();
        foreach (string dir in data["moves"]) {
            Debug.Log(dir);
            if(dir == "left") {
                TwitchMoves.AddLast(Direction.Left);
            } else if (dir == "down") {
                TwitchMoves.AddLast(Direction.Down);
            } else if (dir == "up") {
                TwitchMoves.AddLast(Direction.Up);
            } else if (dir == "right") {
                TwitchMoves.AddLast(Direction.Right);
            } else if (dir == "stand") {
                TwitchMoves.AddLast(Direction.Stand);
            } else {
                Debug.Log("oops");
            }
        }
    }

}
