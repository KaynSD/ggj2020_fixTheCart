using System;
using System.Collections;
using System.Collections.Generic;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
	public PlayerController PrefabInstance;
	public bool GameStarted = false;
	public Dictionary<int, PlayerController> players; 
	public Transform CameraObject;
	void Start()
	{
		players = new Dictionary<int, PlayerController>();

		if(AirConsole.instance.IsAirConsoleUnityPluginReady()) {
			// Create Existing
			List<int> deviceIds = AirConsole.instance.GetControllerDeviceIds();
			foreach(int device in deviceIds) {
				CreatePlayerInstance(device);
			}
		}
		// Add Listeners
		AirConsole.instance.onConnect += AddPlayer;
		AirConsole.instance.onDisconnect += RemovePlayer;
		AirConsole.instance.onMessage += HandleMessage;

	}

	void Update() {
		Vector3 position = new Vector3();
		int count = 0;

		foreach(PlayerController playerController in players.Values) {
			if(playerController != null) {
				position += playerController.transform.position;
				count ++;
			}
		}
		if(count < 1) count = 1;
		position /= count;

		float distance = 0;
		foreach(PlayerController playerController in players.Values) {
			if(playerController != null) {
				float cDistance = Vector3.Distance(playerController.transform.position, position);
				if(distance < cDistance){
					distance = cDistance;
				}
			}
		}
		position.y = Mathf.Sqrt(distance);

		CameraObject.transform.position = position;
	}


	private void HandleMessage(int from, JToken data)
	{
		if(players.ContainsKey(from)) {
			players[from].HandleMessage(data);
		}
	}

	private void AddPlayer(int device_id)
	{

		if(!GameStarted || players.ContainsKey(device_id)) {
			//
			Debug.Log($"Adding Player {device_id}");
			AirConsole.instance.Message(device_id, SetPadState(GameControllerStates.Game));
			CreatePlayerInstance(device_id);
		} else {
			Debug.Log($"Telling Player {device_id} to wait");
			AirConsole.instance.Message(device_id, SetPadState(GameControllerStates.Wait));
		}
	}
	private void RemovePlayer(int device_id)
	{
		if(players.ContainsKey(device_id)) {
			Debug.Log($"Disconnecting Player {device_id}");
			Destroy(players[device_id].gameObject);
		}
	}

	private JToken SetPadState(GameControllerStates state)
	{
		JObject jToken = new JObject();
		jToken.Add("show_view_id", "view-"+(int)state);

		return jToken;
	}

	private void CreatePlayerInstance(int device)
	{
		PlayerController controller = Instantiate(PrefabInstance, new Vector3(
			UnityEngine.Random.Range(-3, 3), 
			0,
			UnityEngine.Random.Range(-3, 3)), 
		Quaternion.identity);

		controller.Device = device;
		controller.transform.Rotate(0, (float)( UnityEngine.Random.Range(1, 8) * 45f), 0);

		if(players.ContainsKey(device)) {
			players[device] = controller;
		} else {
			players.Add(device, controller);
		}
	}

	void OnDestroy(){
	}
}
