/// <summary>
/// 文档作用：
/// 作者：陈鸿
/// 编辑时间：
/// 备注：
/// 脚本位置：每个需要被销毁的子物体身上  
/// 脚本功能：销毁自身
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestorySon : MonoBehaviour {

    // 作为被广播通知的方法，由父物体通知执行  
    public void DestoryMe()
    {
        Destroy(gameObject);
    }

}
