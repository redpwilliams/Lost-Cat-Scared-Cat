using System;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    private static readonly string PrefsPath =
        Path.Combine(Application.persistentDataPath, "data.lcsc");
    
    public static void SavePreferences(Preferences data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(PrefsPath, FileMode.Create);

        formatter.Serialize(stream, data);
        
        stream.Close();
    }

    // TODO - Load preferences on Game Start.
    // If returns null, then save preferences with default data ?
    // Use that to run tutorial
    public static Preferences? LoadPreferences()
    {
        try
        {
            if (!File.Exists(PrefsPath))
            {
               Debug.LogError($"File {PrefsPath} does not exist");
               return null;
            }

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(PrefsPath, FileMode.Open);

            var data = (Preferences)formatter.Deserialize(stream);
            stream.Close();
            
            return data;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            Debug.LogError($"{e}\nUsing default Preferences");
            return new Preferences();
        }
    }
}
