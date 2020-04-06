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
        listener.Prefixes.Add("http://192.168.0.114:4000/");
        listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
        listener.Start();

        listenerThread = new Thread(startListener);
        listenerThread.Start();
    }

    void Update() {		
    }

    private void startListener() {
        while (true) {               
            IAsyncResult result = listener.BeginGetContext(ListenerCallback, listener);
            result.AsyncWaitHandle.WaitOne();
        }
    }

    private void ListenerCallback(IAsyncResult result) {				
        HttpListenerContext context = listener.EndGetContext(result);
        if (context.Request.HttpMethod == "POST") {
            Dictionary<string, string[]> data = new Dictionary<string, string[]>();
            string[] data_pairs = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding).ReadToEnd().Split('&');
            foreach(string pair in data_pairs) {
                string[] tuple = pair.Split('=');
                data.Add(tuple[0], tuple[1].Split('+'));
            };

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
        context.Response.Close();
    }

}
