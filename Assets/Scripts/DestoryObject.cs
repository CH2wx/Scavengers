/// <summary>
/// 文档作用：
/// 作者：陈鸿
/// 编辑时间：
/// 备注：
/// 脚本位置：将此脚本挂载到不会被销毁与不会被隐藏的物体身上  
/// 脚本功能：删除需要删除的物体与其子物体  
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryObject : MonoBehaviour
{
    public static DestoryObject instance;

    private GameObject parentObject;
    private GameObject emptyObject;
    private GameObject exitObject;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Update()
    {
        /*if (GameManager.instance.ReturnDoingSetup())
        {
            if (GameManager.instance.playerFoodPoints < float.Epsilon)
                DestroyPrtAndSon();
        }*/
    }

    //删除墙壁与各个物体的代码
    public void DestroyPrtAndSon()
    {
        if (parentObject == null)
        {
            parentObject = GameObject.Find("Board").gameObject;
            emptyObject = GameObject.Find("Empty").gameObject;
            exitObject = GameObject.Find("Exit(Clone)").gameObject;
        }
        
        //当初设想在更换场景时，通过添加Layer与Tag来删除场景里的所有物体，但是后来忽然发现这种方法需要去遍历场景中所有的物体，十分的麻烦，所以后来又想到了，把实例化的物体添加到一个物体中，这样只要通过删除该物体及其子物体即可
        /*parentObject.layer = LayerMask.NameToLayer("CouldDestory");   
        parentObject.tag = "Board";*/
        
        for (int i = 0; i < parentObject.gameObject.transform.childCount; i++)
            Destroy(parentObject.gameObject.transform.GetChild(i).gameObject);
        Destroy(parentObject.gameObject);

        for (int i = 0; i < emptyObject.gameObject.transform.childCount; i++)
            Destroy(emptyObject.transform.GetChild(i).gameObject);

        Destroy(exitObject.gameObject);
    }

    //给需要删除的物体添加代码
    //如果要添加其他的属性，也能通过AddConponent<属性名>()去实现
    public void AddScripts()
    {
        GameObject gameObject = new GameObject();

        gameObject.AddComponent<DestoryParent>();               //添加绑定脚本

        Destroy(gameObject.GetComponent("DestoryParent"));      //删除脚本
    }
}
