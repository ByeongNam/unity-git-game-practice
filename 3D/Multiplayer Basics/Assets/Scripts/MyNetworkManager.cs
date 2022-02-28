using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
// In NetworkManager.cs, virtual code can be overwritten.
// Client connection(Client): client -> server -> OnStartClient() -> OnClientConnect()
// Client connection(Server): client -> server -> OnServerConnect() -> OnServerReady()
//                            -> (Create Player) -> OnServerAddPlayer
public class MyNetworkManager : NetworkManager
{
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);

        MyNetworkPlayer player = conn.identity.GetComponent<MyNetworkPlayer>();

        player.SetDisplayName($"Player {numPlayers}");

        Color displayColor = new Color (
            Random.Range(0f,1f),
            Random.Range(0f,1f),
            Random.Range(0f,1f));
        player.SetDisplayColor(displayColor);
    }
}

