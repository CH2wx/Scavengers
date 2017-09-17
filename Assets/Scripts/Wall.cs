/// <summary>
/// 文档作用：
/// 作者：陈鸿
/// 编辑时间：
/// 备注：墙壁的破坏与控制
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

    public Sprite dmgSprite;
    public int hp = 4;

    private SpriteRenderer spriteRenderer;
    public AudioClip chopSound1;
    public AudioClip chopSound2;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
	
    public void DamageWall(int loss)
    {
        SoundManager.instance.RandomizeSfx(chopSound1, chopSound2);
        spriteRenderer.sprite = dmgSprite;
        hp -= loss;
        if (hp <= 0)
            gameObject.SetActive(false);
    }

}
