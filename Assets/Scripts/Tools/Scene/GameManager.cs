using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private static GameManager instance;
    public static GameManager Instance => instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            MarkPersistentObjects();
        }
        else
        {
            if (instance == this)
            {
                //这里如果发现有新的管理器，则说明回到了之前的场景
                //这时候需要保证单一，且要销毁上一次的对象
                CleanUoAndDestory();
                Destroy(gameObject);
            }
        }

    }


    //所有过场景不被销毁的对象
    public GameObject[] persistentObjs;

    //进入场景时，需要将目标对象保留到下一个场景
    private void MarkPersistentObjects()
    {
        foreach (var obj in persistentObjs)
        {
            if (obj != null)
            {
                DontDestroyOnLoad(obj);
            }
        }
    }

    //进入另一个场景时，需要销毁上一个场景的对象
    private void CleanUoAndDestory()
    {
        foreach (var obj in persistentObjs)
        {
            Destroy(obj);
        }
        Destroy(gameObject);
    }
}