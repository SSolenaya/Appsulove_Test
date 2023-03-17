using System;
using UnityEngine;

namespace Assets.Scripts
{

    public static class SaveManager
    {
        private static readonly string _keyPlayerData = "PlayerDataKey";
        public static PlayerData _playerData;

        public static PlayerData GetSavedData()
        {
            string json = PlayerPrefs.GetString(_keyPlayerData, "");
            _playerData = json == "" ? new PlayerData() : JsonUtility.FromJson<PlayerData>(json);
            return _playerData;
        }

        public static void Save(PlayerData playerData)
        {
            string json = JsonUtility.ToJson(playerData);
            PlayerPrefs.SetString(_keyPlayerData, json);
            PlayerPrefs.Save();
        }
    }
}