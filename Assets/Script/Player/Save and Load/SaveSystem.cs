using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SavePlayer(Player player, SceneTransition scene)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.txt";
        FileStream fileStream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(player, scene);

        formatter.Serialize(fileStream, data);
        fileStream.Close();

        if (!File.Exists(path))
        {
            Debug.Log("Not Saved");
        }
        else Debug.Log("Saved");
    }

    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.txt";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(fileStream) as PlayerData;
            fileStream.Close();

            return data;

        }
        else
        {
            Debug.Log("File not exist");
            return null;
        }
    }

    public static void ClearData()
    {
        string path = Application.persistentDataPath + "/player.txt";
        File.Delete(path);
    }
}
