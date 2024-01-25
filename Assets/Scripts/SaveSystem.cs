using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

///Uses serialization to save and load different types of data
///probably should have found a more efficent way of saving that didnt
///require me to make separate files of everything
public static class SaveSystem
{
    public static bool highscore;
    public static void CompareDistance(int currentDistance, int loadedDistance)
    {
        if (currentDistance > loadedDistance)
        {
            string savename = "distance";
            SaveData(currentDistance, savename);
            highscore = true;
        }
        else
        {
            highscore = false;
        }
    }

    public static void AddCoins(int newCoins, int loadedCoins)
    {
        loadedCoins += newCoins;
        string savename = "coins";
        SaveData(loadedCoins, savename);
    }

    public static void SavePlayer(float speed, float maxDuration)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.txt";
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, speed);
        formatter.Serialize(stream, maxDuration);
        stream.Close();
    }
    public static void LoadPlayer(MovePlayer player)
    {
        string path = Application.persistentDataPath + "/player.txt";
        string path2 = Application.persistentDataPath + "/options.txt";
        if (File.Exists(path) && File.Exists(path2))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            FileStream stream2 = new FileStream(path2, FileMode.Open);
            player.speed = (float)formatter.Deserialize(stream);
            player.maxDuration = (float)formatter.Deserialize(stream);
            player.motionBlurOn = (bool)formatter.Deserialize(stream2);
            stream.Close();
        }
        else
        {
            Debug.LogError("No save file in " + path);
        }
    }
    public static void SaveData(int value, string saveType)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + saveType + ".txt";
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, value);
        stream.Close();
    }
    public static int LoadData(string filename)
    {
        string path = Application.persistentDataPath + "/" + filename + ".txt";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            int data = (int)formatter.Deserialize(stream);

            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("No save file in " + path);
            return 0;
        }
    }
    public static void SaveAudio(float main,float sfx,float music)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/audio.txt";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, main);
        formatter.Serialize(stream, sfx);
        formatter.Serialize(stream, music);
        stream.Close();
        Debug.Log("Saved audio main " + main + " sfx " + sfx + " music " + music);
    }
    public static void SaveOptions(bool motionBlur, int aaMode)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/options.txt";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, motionBlur);
        formatter.Serialize(stream, aaMode);
        stream.Close();
    }
    public static void LoadOptions(OptionsMenu options)
    {
        string path = Application.persistentDataPath + "/options.txt";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            options.motionBlurOn = (bool)formatter.Deserialize(stream);
            options.AAmode = (int)formatter.Deserialize(stream);
            stream.Close();

        }
        else
        {
            Debug.LogError("No save file in " + path);
        }
    }
    public static void LoadAudio(AudioManager audio)
    {
        string path = Application.persistentDataPath + "/audio.txt";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            audio.mainVolume = (float)formatter.Deserialize(stream);
            audio.sfxVolume = (float)formatter.Deserialize(stream);
            audio.musicVolume = (float)formatter.Deserialize(stream);

            stream.Close();

        }
        else
        {
            Debug.LogError("No save file in " + path);
        }
    }
    public static void SaveShop(int speedLvl, int speedCost,bool[]boughtskins, int nitroLvl, int nitroCost)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/shop.txt";
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, speedLvl);
        formatter.Serialize(stream, speedCost);
        formatter.Serialize(stream, boughtskins);
        formatter.Serialize(stream, nitroLvl);
        formatter.Serialize(stream, nitroCost);
        stream.Close();
    }
    public static void LoadShop(Upgrades shop)
    {
        string path = Application.persistentDataPath + "/shop.txt";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            shop.speedLvl = (int)formatter.Deserialize(stream);
            shop.speedCost = (int)formatter.Deserialize(stream);
            shop.boughtSkin = (bool[])formatter.Deserialize(stream);
            shop.nitroLvl = (int)formatter.Deserialize(stream);
            shop.nitroCost = (int)formatter.Deserialize(stream);
            stream.Close();
        }
        else
        {
            Debug.LogError("No save file in " + path);
        }
    }
}
