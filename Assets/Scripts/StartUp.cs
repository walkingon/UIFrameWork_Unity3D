using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUp : MonoBehaviour
{

    void Start()
    {
        ResourceManager.getInstance().Init();
        ViewManager.getInstance().Init();

    }

}
