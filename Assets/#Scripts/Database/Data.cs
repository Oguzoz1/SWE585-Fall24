using Database.Payload;

namespace Database.Data
{
    [System.Serializable]
    public class PlayerData
    {
        public int PlayerId { get; private set; }
        public int UserCredentialsId { get; private set; }
        public string PlayerName { get; private set; }
        public string LoginName { get; private set; }


        public PlayerData SetPlayerDataByPlayerPayload(PlayerPayload payload)
        {
            PlayerId = payload.id;
            UserCredentialsId = payload.userCredentials.id;
            LoginName = payload.userCredentials.loginName;
            PlayerName = payload.playerName;

            return this;
        }
    }
}