﻿using Hevadea.Worlds.Level;

namespace Hevadea.GameManager
{
    public partial class GameManager
    {
        public string GetRemotePlayerPath()
            => $"{SavePath}/remotes_players";
        
        public string GetGameSaveFile()
            => $"{SavePath}/game.json";
        
        public string GetPlayerSaveFile()
            => $"{SavePath}/player.json";
        
        public string GetLevelSavePath(Level level)
            => $"{SavePath}/{level.Name}.json";
        
        public string GetLevelMinimapSavePath(Level level)
            => $"{SavePath}/{level.Name}-minimap.png";
        
        public string GetLevelMinimapDataPath(Level level)
            => $"{SavePath}/{level.Name}-minimap.json";
        
        public string GetLevelSavePath(string level)
            => $"{SavePath}/{level}.json";
    }
}