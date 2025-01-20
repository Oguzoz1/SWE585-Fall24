using Database.Payload;
using Game.Data;
using System;
using System.Linq;

namespace Database.Utility
{
    public static class Mapper
    {
        public static TTarget MapTo<TSource, TTarget>(this TSource source)
            where TSource : class
            where TTarget : class, new()
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var target = new TTarget();
            var sourceProperties = typeof(TSource).GetProperties();
            var targetProperties = typeof(TTarget).GetProperties();

            foreach (var sourceProp in sourceProperties)
            {
                if (!sourceProp.CanRead) continue;

                var targetProp = targetProperties.FirstOrDefault(p => p.Name == sourceProp.Name && p.PropertyType == sourceProp.PropertyType);
                if (targetProp != null && targetProp.CanWrite)
                {
                    var value = sourceProp.GetValue(source);
                    targetProp.SetValue(target, value);
                }
            }

            return target;
        }

        public static PlayerData mapToPlayerData(this PlayerData data, PlayerPayload load)
        {
            data.LoginName = load.userCredentials.loginName;
            data.UserCredentialsId = load.userCredentials.id;
            data.Role = load.userCredentials.role;
            data.PlayerId = load.id;
            data.PlayerName = load.playerName;
            return data;
        }
    }

}
