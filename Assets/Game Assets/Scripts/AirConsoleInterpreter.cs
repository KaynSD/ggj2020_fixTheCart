using System.Collections;
using System.Collections.Generic;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class AirConsoleInterpreter : MonoBehaviour
{
	void Start() {
		AirConsole.instance.onMessage += OnMessage;
		AirConsole.instance.onConnect += OnConnect;
		AirConsole.instance.onDisconnect += OnDisconnect;

		DontDestroyOnLoad(this);
	}

	void OnMessage(int from, JToken data) {
		//Debug.Log($"{from}: {data.ToString()}");
	}
	void OnConnect(int from) {
		//Debug.Log($"{from}: connects");
	}
	void OnDisconnect(int from) {
		//Debug.Log($"{from}: disconnects");
	}
}
