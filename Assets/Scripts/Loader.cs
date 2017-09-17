/// <summary>
/// 文档作用：
/// 作者：陈鸿
/// 编辑时间：
/// 备注：检查“GameManager”是否被实例化
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour {

    public GameObject gameManager;
    public GameObject soundManager;

	void Awake () {
        if (GameManager.instance == null)
            Instantiate(gameManager);

        if (SoundManager.instance == null)
            Instantiate(soundManager);
	}
	
}
