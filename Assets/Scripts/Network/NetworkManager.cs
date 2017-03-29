using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;
using System.Text;

namespace MyGameClient {
	public class NetworkManager : MonoBehaviour {
		public string hostname = "127.0.0.1";
		public int port = 41234;

		private UdpClient udpClient;

		// Use this for initialization
		void Start () {
			// Register event listeners
			Movement.OnMovement += SendPlayerMovement;

			try {
				udpClient = new UdpClient ();
				udpClient.Connect (hostname, port);
			} catch (Exception e) {
				Debug.Log (e.ToString ());
			}
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		void SendPlayerMovement (Player.PlayerPosition playerPosition) {
			string jsonString = JsonUtility.ToJson (playerPosition);
			byte[] data = Encoding.UTF8.GetBytes (jsonString);
			udpClient.Send (data, data.Length);
		}
	}
}
