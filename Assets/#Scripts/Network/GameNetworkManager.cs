using Game.Player;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Network
{
    public class GameNetworkManager : NetworkManager
    {
        [Header("Player Settings")]
        [SerializeField] private PlayerDataSO _playerDataSO;


        public void LaunchGame()
        {
            Debug.Log("CLICKED");
            StartClient();
        }

        public override void OnClientConnect()
        {
            base.OnClientConnect();
            Debug.Log("Client connected to the server!");

        }

        public override void OnServerSceneChanged(string newSceneName)
        {
            // Since we name our maps with Scene_Map, we compare.
            if (SceneManager.GetActiveScene().name.StartsWith("MainGame"))
            {
                // ADD NETWORKED SERVICES HERE: FOR EXAMPLE: GAMEOVERHANDLER
            }
        }

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            base.OnServerAddPlayer(conn);
            Debug.Log($"Player connected: {conn.connectionId}");
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            Debug.Log($"Server started on address {networkAddress} and port {transport.ServerActive()}");
        }

    }
}
