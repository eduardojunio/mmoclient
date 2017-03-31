using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGameClient {
	public class PlayerSyncMessage : Message {
		public Dictionary<string, Player> onlinePlayers;

		public PlayerSyncMessage(Dictionary<string, Player> onlinePlayers, string type = "sync")
			: base(type) {
			this.onlinePlayers = onlinePlayers;
		}
	}
}
