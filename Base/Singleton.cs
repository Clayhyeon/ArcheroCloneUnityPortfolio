using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
   private static T _instance;
   public static T Instance
   {
      get
      {
         if (_instance is not null)
         {
            return _instance;
         }

         Init();
         return _instance;
      }
   }
   private static void Init()
   {
      if (_instance is not null)
      {
         return;
      }
            
      var obj = GameObject.Find(typeof(T).Name);
           
      if (obj is null)
      {
         obj = new GameObject {  name = typeof(T).Name };
         obj.AddComponent<T>();
      }
            
      DontDestroyOnLoad(obj);
      _instance = obj.GetComponent<T>();
   }
   
}
