using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Http;

public class UnityHttpClient : MonoBehaviour {

    private HttpClient client;

    // Start is called before the first frame update
    void Start() {
        client = new HttpClient();
    }

    void Update() {}

    async void sendMinimapArray() {
        var values = new Dictionary<string, string>();
        // var values = 
        //     [
        //         [
        //             {type: 0, ''}, 
        //             {type: 1, 'abc'}, 
        //             {type: 0, ''}, 
        //             {type: 0, ''}
        //         ],
        //         [
        //             {type: 0, ''}, 
        //             {type: 2, 'def'}, 
        //             {type: 0, ''}, 
        //             {type: 0, ''}
        //         ],
        //         [
        //             {type: 0, ''}, 
        //             {type: 0, ''}, 
        //             {type: 0, ''}, 
        //             {type: 0, ''}
        //         ],
        //         [
        //             {type: 0, ''}, 
        //             {type: 0, ''}, 
        //             {type: 3, 'streamer'}, 
        //             {type: 0, ''}
        //         ]
        //     ]
        // };

        FormUrlEncodedContent content = new FormUrlEncodedContent(values);
        var response = await client.PostAsync("http://127.0.0.1:5000", content);
        var responseString = await response.Content.ReadAsStringAsync();
    }
}
