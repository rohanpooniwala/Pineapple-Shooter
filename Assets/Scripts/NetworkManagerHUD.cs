#if ENABLE_UNET

namespace UnityEngine.Networking
{
	[AddComponentMenu("Network/NetworkManagerHUD")]
	[RequireComponent(typeof(NetworkManager))]
	[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
	public class NetworkManagerHUD : MonoBehaviour
	{
		public NetworkManager manager;
		[SerializeField] public bool showGUI = true;
		[SerializeField] public int offsetX;
		[SerializeField] public int offsetY;

		// Runtime variable
		bool showServer = false;

		void Awake()
		{
			manager = GetComponent<NetworkManager>();
		}

		void Update()
		{
			if (!showGUI)
				return;

			if (!NetworkClient.active && !NetworkServer.active && manager.matchMaker == null)
			{
				if (Input.GetKeyDown(KeyCode.S))
				{
					manager.StartServer();
				}
				if (Input.GetKeyDown(KeyCode.H))
				{
					manager.StartHost();
				}
				if (Input.GetKeyDown(KeyCode.C))
				{
					manager.StartClient();
				}
			}
			if (NetworkServer.active && NetworkClient.active)
			{
				if (Input.GetKeyDown(KeyCode.X))
				{
					manager.StopHost();
				}
			}
		}

		void OnGUI()
		{
			if (!showGUI)
				return;

			int height = Screen.height/10;
			int width = Screen.width/4;

			int xpos = 10 + offsetX;
			int ypos = 40 + offsetY;
			int spacing = height+5;

			if (!NetworkClient.active && !NetworkServer.active && manager.matchMaker == null)
			{
				if (GUI.Button(new Rect(xpos, ypos, width, height), "LAN Host(H)"))
				{
					manager.StartHost();
				}
				ypos += spacing;

				if (GUI.Button(new Rect(xpos, ypos, width/2, height), "LAN Client(C)"))
				{
					manager.StartClient();
				}
				manager.networkAddress = GUI.TextField(new Rect(xpos + width/2, ypos, width/2, height), manager.networkAddress);
				ypos += spacing;

				if (GUI.Button(new Rect(xpos, ypos, width, height), "LAN Server Only(S)"))
				{
					manager.StartServer();
				}
				ypos += spacing;
			}
//			else
//			{
//				if (NetworkServer.active)
//				{
//					GUI.Label(new Rect(xpos, ypos, width, height), "Server: port=" + manager.networkPort);
//					ypos += spacing;
//				}
//				if (NetworkClient.active)
//				{
//					GUI.Label(new Rect(xpos, ypos, width, height), "Client: address=" + manager.networkAddress + " port=" + manager.networkPort);
//					ypos += spacing;
//				}
//			}

			if (NetworkClient.active && !ClientScene.ready)
			{
				if (GUI.Button(new Rect(xpos, ypos, width, height), "Client Ready"))
				{
					ClientScene.Ready(manager.client.connection);
				
					if (ClientScene.localPlayers.Count == 0)
					{
						ClientScene.AddPlayer(0);
					}
				}
				ypos += spacing;
			}

			if (NetworkServer.active || NetworkClient.active)
			{
				if (GUI.Button(new Rect(xpos, ypos, width, height), "Stop (X)"))
				{
					manager.StopHost();
				}
				ypos += spacing;
			}

			if (!NetworkServer.active && !NetworkClient.active)
			{
				ypos += 10;

				if (manager.matchMaker == null)
				{
					if (GUI.Button(new Rect(xpos, ypos, width, height), "Enable Match Maker (M)"))
					{
						manager.StartMatchMaker();
					}
					ypos += spacing;
				}
				else
				{
					if (manager.matchInfo == null)
					{
						if (manager.matches == null)
						{
							if (GUI.Button(new Rect(xpos, ypos, width, height), "Create Internet Match"))
							{
								manager.matchMaker.CreateMatch(manager.matchName, manager.matchSize, true, "", manager.OnMatchCreate);
							}
							ypos += spacing;

							GUI.Label(new Rect(xpos, ypos, width/2, height), "Room Name:");
							manager.matchName = GUI.TextField(new Rect(xpos+width/2, ypos, width/2, height), manager.matchName);
							ypos += spacing;

							ypos += 10;

							if (GUI.Button(new Rect(xpos, ypos, width, height), "Find Internet Match"))
							{
								manager.matchMaker.ListMatches(0,20, "", manager.OnMatchList);
							}
							ypos += spacing;
						}
						else
						{
							foreach (var match in manager.matches)
							{
								if (GUI.Button(new Rect(xpos, ypos, width, height), "Join Match:" + match.name))
								{
									manager.matchName = match.name;
									manager.matchSize = (uint)match.currentSize;
									manager.matchMaker.JoinMatch(match.networkId, "", manager.OnMatchJoined);
								}
								ypos += spacing;
							}
						}
					}

					if (GUI.Button(new Rect(xpos, ypos, width, height), "Change MM server"))
					{
						showServer = !showServer;
					}
					if (showServer)
					{
						ypos += spacing;
						if (GUI.Button(new Rect(xpos, ypos, width/2, height), "Local"))
						{
							manager.SetMatchHost("localhost", 1337, false);
							showServer = false;
						}
						ypos += spacing;
						if (GUI.Button(new Rect(xpos, ypos, width/2, height), "Internet"))
						{
							manager.SetMatchHost("mm.unet.unity3d.com", 443, true);
							showServer = false;
						}
						ypos += spacing;
							if (GUI.Button(new Rect(xpos, ypos, width/2, height), "Staging"))
						{
							manager.SetMatchHost("staging-mm.unet.unity3d.com", 443, true);
							showServer = false;
						}
					}

					ypos += spacing;

							GUI.Label(new Rect(xpos, ypos, width, height), "MM Uri: " + manager.matchMaker.baseUri);
					ypos += spacing;

							if (GUI.Button(new Rect(xpos, ypos, width, height), "Disable Match Maker"))
					{
						manager.StopMatchMaker();
					}
					ypos += spacing;
				}
			}
		}
	}
};
#endif //ENABLE_UNET
