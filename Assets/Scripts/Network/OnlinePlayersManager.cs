using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using UnityEngine.UI;

namespace MyGameClient {
	public class OnlinePlayersManager : MonoBehaviour {
		public GameObject playerPrefab;

		private Dictionary<GameObject, Player> localPlayers;

		void Start () {
			localPlayers = new Dictionary<GameObject, Player> ();

			NetworkManager.OnSyncPlayers += SyncLocalPlayers;
		}

		void SyncLocalPlayers() {
		}

		GameObject CreatePlayer() {
			return (GameObject)Instantiate (playerPrefab);
		}

		void Update () {
			List<string> onlinePlayersKeys = new List<string> (NetworkManager.onlinePlayers.Keys);

			foreach(string onlinePlayersKey in onlinePlayersKeys) {
				if (!localPlayers.ContainsValue (NetworkManager.onlinePlayers[onlinePlayersKey]) && onlinePlayersKey != NetworkManager.myFullAddress) {
					localPlayers.Add (CreatePlayer (), NetworkManager.onlinePlayers[onlinePlayersKey]);
				}
			}

			List<GameObject> localPlayersKeys = new List<GameObject> (localPlayers.Keys);

			foreach(GameObject localPlayersKey in localPlayersKeys) {
				if (!NetworkManager.onlinePlayers.ContainsValue (localPlayers[localPlayersKey])) {
					localPlayers.Remove (localPlayersKey);
					Destroy (localPlayersKey);
				} else {
					Vector3 newPosition = localPlayersKey.transform.position;
					newPosition.x = localPlayers[localPlayersKey].coordinates.x;
					newPosition.y = localPlayers[localPlayersKey].coordinates.y;
					localPlayersKey.transform.position = newPosition;
				}
			}
		}
	}
}
