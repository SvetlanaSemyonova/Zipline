using UnityEngine;

public class menubg : MonoBehaviour
  
{
    public static menubg instance;
    void Awake()
    {


        instance = this;

        DontDestroyOnLoad(this.gameObject);



    }
}
