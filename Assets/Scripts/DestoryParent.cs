/// <summary>
/// 文档作用：
/// 作者：陈鸿
/// 编辑时间：2017/9/13
/// 备注：
/// 脚本位置：将此脚本挂载到父物体A身上  
/// 脚本功能：发送广播给自己和自己的所有子物体  
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryParent : MonoBehaviour {

    void Start()
    {
        // 执行Start方法后会发送一个广播  
        // 广播会从自身开始查找这个DestoryMe方法，查找完自身后会查找所有子物体  
        BroadcastMessage("DestoryMe");
    }
}
