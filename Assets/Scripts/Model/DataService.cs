using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using SQLite;
public class DataService : Singleton<DataService>
{
  public SQLiteConnection _connection;
  [SerializeField] string databaseName;

  void Start()
  {
#if UNITY_EDITOR
    var dbPath = string.Format(@"Assets/StreamingAssets/{0}", databaseName);
#else
    var filepath = string.Format("{0}/{1}", Application.persistentDataPath, databaseName);
    if (!File.Exists(filepath))
    {
      Debug.Log("Database not in Persistent path");
  
#if UNITY_ANDROID
      var loadDb = new WWW("jar:file://" + Application.dataPath + "!/assets/" + databaseName);
      while (!loadDb.isDone) { }  
      File.WriteAllBytes(filepath, loadDb.bytes);
#elif UNITY_IOS
      var loadDb = Application.dataPath + "/Raw/" + databaseName; 

      File.Copy(loadDb, filepath);
#elif UNITY_WP8
      var loadDb = Application.dataPath + "/StreamingAssets/" + databaseName; 
      File.Copy(loadDb, filepath);

#elif UNITY_WINRT
      var loadDb = Application.dataPath + "/StreamingAssets/" + databaseName;
      File.Copy(loadDb, filepath);
		
#elif UNITY_STANDALONE_OSX
      var loadDb = Application.dataPath + "/Resources/Data/StreamingAssets/" + databaseName;
      File.Copy(loadDb, filepath);
#else
      var loadDb = Application.dataPath + "/StreamingAssets/" + databaseName;
      File.Copy(loadDb, filepath);

#endif

      Debug.Log("Database written");
    }

    var dbPath = filepath;
#endif
    _connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
    Debug.Log("Final PATH: " + dbPath);
  }

}
