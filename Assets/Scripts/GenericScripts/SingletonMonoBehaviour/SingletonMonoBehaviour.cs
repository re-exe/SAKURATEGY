using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Singleton inherited from MonoBehaviour.
/// </summary>
/// <typeparam name="T"></typeparam>
public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour{

    /// <summary>
    /// Instance
    /// </summary>
    private static volatile T instance;

    /// <summary>
    /// Synchronous Object
    /// </summary>
    /// <returns></returns>
    private static object syncObject = new object();

    /// <summary>
    /// Instance getter/setter
    /// </summary>
    /// <value></value>
    public static T Instance{
        get{

            // Prevents the creation of an object when an instance is called again at the end of the application.
            if(applicationIsQuitting){
                return null;
            }

            // Look for instances when there are none.
            if(instance == null){
                instance = FindObjectOfType<T>() as T;

                // If there are multiple instances.
                if(FindObjectsOfType<T>().Length > 1){
                    return instance;
                }

                // If not found by Find, a new object is created.
                if(instance == null){
                    // lock to avoid calling instantiation at the same time.
                    lock(syncObject){
                        GameObject singleton = new GameObject();
                        // Set a name so that it is easy to recognize it as a singleton object.
                        singleton.name = typeof(T).ToString() + "(singleton)";
                        instance = singleton.AddComponent<T>();

                        // Do not let them be discarded when changing scenes.
                        DontDestroyOnLoad(singleton);
                    }
                }
            }
            return instance;
        }

        // It is used to null the instance, so it should be private.
        private set{
            instance = value;
        }
    }

    /// <summary>
    /// Determine if the application is closed.
    /// </summary>
    static bool applicationIsQuitting = false;

    private void OnApplicationQuit(){
        applicationIsQuitting = true;
    }
    
    private void OnDestroy() {
        Instance = null;
    }

    /// <summary>
    /// Constructor
    /// Setting it to "protected" disables the creation of instances.
    /// </summary>
    protected SingletonMonoBehaviour(){}
}
