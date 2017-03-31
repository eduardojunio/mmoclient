using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Net.Sockets;
using System;
using System.Text;
using Newtonsoft.Json;

namespace MyGameClient {
	public class NetworkManager : MonoBehaviour {
		public delegate void Connected();
		public static event Connected OnConnected;
		public delegate void SyncPlayers();
		public static event SyncPlayers OnSyncPlayers;

		public static Dictionary<string, Player> onlinePlayers = new Dictionary<string, Player> ();
		public static string myFullAddress;

		public string hostname = "127.0.0.1";
		public int port = 41234;
		public GameObject player;

		private UdpClient udpClient;

		void Start () {
			Application.runInBackground = true;

			try {
				udpClient = new UdpClient ();
				udpClient.Connect (hostname, port);
			} catch (Exception e) {
				Debug.Log (e.ToString ());
			}

			myFullAddress = udpClient.Client.LocalEndPoint.ToString ();
			Text playerText = player.GetComponentInChildren<Text> ();
			playerText.text = myFullAddress;

			Movement.OnMovement += SendPlayerMovement;

			PlayerPositionMessage connectMessage = new PlayerPositionMessage (type: "connect");
			SendToServer (connectMessage);

			udpClient.BeginReceive (new AsyncCallback(OnMessageCallback), null);
		}

		void OnMessageCallback(IAsyncResult asyncResult) {
			IPEndPoint serverEp = new IPEndPoint (IPAddress.Any, port);
			byte[] bytesReceived = udpClient.EndReceive (asyncResult, ref serverEp);
			string jsonString = Encoding.UTF8.GetString (bytesReceived);

			Message message = JsonConvert.DeserializeObject<Message> (jsonString);

			switch (message.type) {
			case "sync":
				Dictionary<string, Player> newPositions = JsonConvert.DeserializeObject<PlayerSyncMessage> (jsonString).onlinePlayers;
				foreach (KeyValuePair<string, Player> newPlayerPosition in newPositions) {
					onlinePlayers[newPlayerPosition.Key].coordinates.x = newPlayerPosition.Value.coordinates.x;
					onlinePlayers[newPlayerPosition.Key].coordinates.y = newPlayerPosition.Value.coordinates.y;
				}

				if (OnSyncPlayers != null) {
					OnSyncPlayers ();
				}
				break;
			case "syncOnlinePlayers":
				onlinePlayers = JsonConvert.DeserializeObject<PlayerSyncMessage> (jsonString).onlinePlayers;
				break;
			case "connected":
				Player playerConnected = JsonConvert.DeserializeObject<PlayerConnectedMessage> (jsonString).player;
				string playerKey = playerConnected.ipAddress + ":" + playerConnected.portNumber;
				onlinePlayers.Add (playerKey, playerConnected);

				if (OnConnected != null) {
					OnConnected ();
				}
				break;
			case "disconnected":
				Player playerDisconnected = JsonConvert.DeserializeObject<PlayerConnectedMessage> (jsonString).player;
				string disconnectedPlayerKey = playerDisconnected.ipAddress + ":" + playerDisconnected.portNumber;
				onlinePlayers.Remove (disconnectedPlayerKey);
				break;
			}

			udpClient.BeginReceive (new AsyncCallback(OnMessageCallback), null);
		}

		void OnApplicationQuit() {
			Message disconnectMessage = new Message ("disconnect");
			SendToServer (disconnectMessage);
		}

		void SendToServer(object msg) {
			string jsonString = JsonUtility.ToJson (msg);
			byte[] data = Encoding.UTF8.GetBytes (jsonString);
			udpClient.Send (data, data.Length);
		}

		void SendPlayerMovement (PlayerPositionMessage playerPosition) {
			SendToServer (playerPosition);
		}
	}
}
