using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using static FileManager;

public class FileManager : MonoBehaviour
{
    public const string saveFileExtension = "save";

    private static Texture2D missingTexture;

    public static Texture2D TextureFromPNG(string texturePath)
    {
        byte[] textureBytes;

        if (File.Exists(Application.streamingAssetsPath + $"/textures/{texturePath}"))
        {
            textureBytes = File.ReadAllBytes(Application.streamingAssetsPath + $"/textures/{texturePath}");
            
        }
        else
        {
            print($"Missing Texture for '{texturePath}'");
            textureBytes = File.ReadAllBytes(Application.streamingAssetsPath + $"/textures/missing.png");
        }

        Texture2D texture = new Texture2D(1, 1);
        texture.LoadImage(textureBytes);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        return texture;
    }

    public static Texture2D MissingTexture()
    {
        if(missingTexture == null)
        {
            byte[] textureBytes = File.ReadAllBytes(Application.streamingAssetsPath + $"/textures/missing.png");
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(textureBytes);
            texture.filterMode = FilterMode.Point;
            texture.wrapMode = TextureWrapMode.Clamp;
            missingTexture = texture;
            return missingTexture;
        }
        else
        {
            return missingTexture;
        }
    }

    public static Save[] ReadSaveFiles()
    {
        string[] filePaths = System.IO.Directory.GetFiles(Application.streamingAssetsPath + "/saves");
        if (filePaths.Length == 0) return null;

        List<Save> saves = new List<Save>();

        foreach (string filePath in filePaths)
        {
            if (!Path.GetFileName(filePath).EndsWith($".{saveFileExtension}")) continue;
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(filePath, FileMode.Open);
            Save save = (Save)formatter.Deserialize(file);
            file.Close();
            saves.Add(save);
        }

        return saves.ToArray();
    }

    public static Save ReadSaveFile(string saveName)
    {
        if (DoesSaveExist(saveName))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(Application.streamingAssetsPath + $"/saves/{saveName}.{saveFileExtension}", FileMode.Open);
            Save save = (Save)formatter.Deserialize(file);
            file.Close();
            return save;
        }

        return null;
    }

    public static void WriteSaveFile(Save save)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        if (DoesSaveExist(save.saveName))
        {
            FileStream existingFile = File.Open(Application.streamingAssetsPath + $"/saves/{save.saveName}.{saveFileExtension}", FileMode.Open);
            Save existingSave = (Save)formatter.Deserialize(existingFile);
            existingFile.Close();
            save.saveDateTime = existingSave.saveDateTime;
        }

        FileStream file = File.Create(Application.streamingAssetsPath + $"/saves/{save.saveName}.{saveFileExtension}");
        formatter.Serialize(file, save);
        file.Close();
    }

    public static void DeleteSaveFile(string saveName)
    {
        if (DoesSaveExist(saveName))
        {
            File.Delete(Application.streamingAssetsPath + $"/saves/{saveName}.{saveFileExtension}");
        }
    }

    public static bool DoesSaveExist(string saveName)
    {
        return File.Exists(Application.streamingAssetsPath + $"/saves/{saveName}.{saveFileExtension}");
    }

    [Serializable]
    public class Save
    {
        public string saveName;
        public string saveSeed;
        public DateTime saveDateTime;
        public List<PlanetData> planetDatas = new List<PlanetData>();
        public string activePlanetID;
        public Player[] players;

        public Save(string saveName, string saveSeed, Planet[] planets, string activePlanetID, Player[] players)
        {
            foreach (Planet planet in planets)
            {
                this.saveName = saveName;
                this.saveSeed = saveSeed;
                this.activePlanetID = activePlanetID;

                saveDateTime = DateTime.Now;

                List<ChunkData> chunkDatas = new List<ChunkData>();

                this.players = players;

                foreach (Chunk chunk in planet.allChunks)
                {
                    //chunkDatas.Add(new ChunkData(chunk.startCoords, chunk.GetTileData().Serialize()));
                }

                planetDatas.Add(new PlanetData(planet.planetID, planet.planetDisplayName, planet.GetType(), chunkDatas));
            }

        }

        [Serializable]
        public class ChunkData
        {
            public SerializableVector2 position;
            public SerializableDictionary<SerializableVector2, Tile> tileData;

            public ChunkData(Vector2 position, SerializableDictionary<SerializableVector2, Tile> tileData)
            {
                this.position = new SerializableVector2(position.x, position.y);
                this.tileData = tileData;
            }
        }

        [Serializable]
        public class PlanetData
        {
            public string planetID;
            public string planetDisplayName;
            public System.Type planetType;
            public List<ChunkData> chunkDatas;

            public PlanetData(string planetID, string planetDisplayName, System.Type planetType, List<ChunkData> chunksData)
            {
                this.planetID = planetID;
                this.planetDisplayName = planetDisplayName;
                this.planetType = planetType;
                this.chunkDatas = chunksData;
            }
        }
    }


}
