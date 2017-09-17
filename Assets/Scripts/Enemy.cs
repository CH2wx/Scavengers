/// <summary>
/// 文档作用：
/// 作者：陈鸿
/// 编辑时间：9.6
/// 备注：敌人
/// </summary>
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovinObject
{

    public int playerDamage;

    private Animator animator;
    private Transform target;
    private bool skipMove;   //让敌人每回合只移动一次
    public AudioClip enemyAttack1;
    public AudioClip enemyAttack2;

    protected override void Start()
    {
        GameManager.instance.AddEnemyToList(this);

        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        if (skipMove)
        {
            skipMove = false;
            return;
        }

        base.AttemptMove<T>(xDir, yDir);
        skipMove = true;
    }

    public void MoveEnemy()
    {
        int xDir = 0;
        int yDir = 0;

        if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
            yDir = target.position.y - transform.position.y > 0 ? 1 : -1;
        else
            xDir = target.position.x - transform.position.x > 0 ? 1 : -1;

        AttemptMove<Player>(xDir, yDir);
    }

    protected override void OnCantMove<T>(T component)
    {
        Player hitPlayer = component as Player;
        hitPlayer.LoseFood(playerDamage);
        animator.SetTrigger("Enemy1Attrack");
        SoundManager.instance.RandomizeSfx(enemyAttack1, enemyAttack2);
    }
}
