using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Http;

public interface minimapSquare {
    int type {get;}
    string id {get;}
}

public class UnityHttpClient : MonoBehaviour {

    private HttpClient client;

    void Start() {
        client = new HttpClient();
    }

    public async void sendMinimapArray(string serialized_board) {
        print("sendMinimapArray");
        var values = new Dictionary<string, string>();
        values["map"] = serialized_board;
        FormUrlEncodedContent content = new FormUrlEncodedContent(values);
        HttpResponseMessage response = await client.PostAsync("http://127.0.0.1:5000/api/update_minimap", content);
        await response.Content.ReadAsStringAsync();
    }
}
