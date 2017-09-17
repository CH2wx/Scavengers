/// <summary>
/// 文档作用：
/// 作者：陈鸿
/// 编辑时间：
/// 备注：
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//abstract 可以让我们创建不完整的类和类成员，并且必须在派生类中实现
public abstract class MovinObject : MonoBehaviour
{

    public float moveTime = 0.1f;
    public LayerMask blokingLayer;

    private BoxCollider2D bc2D;
    private Rigidbody2D rd2D;
    private float inversMoveTime;   //让移动计算更有频率

    protected virtual void Start()         //方便被类重写，当继承的类有不同的实现时，很实用
    {
        bc2D = GetComponent<BoxCollider2D>();
        rd2D = GetComponent<Rigidbody2D>();
        inversMoveTime = 1f / moveTime;
    }

    protected bool Move(int xDir, int yDir, out RaycastHit2D hit)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);

        bc2D.enabled = false;       //禁用自身的碰撞器，防止射线碰撞到自身的碰撞器
        hit = Physics2D.Linecast(start, end, blokingLayer); //从start开始出发，把接触的值储存在hit中返回
        bc2D.enabled = true;        //激活碰撞器

        if (hit.transform == null)     //可以移动
        {
            StartCoroutine(SmoothMovement(end));
            return true;
        }

        return false;
    }

    protected virtual void AttemptMove<T>(int xDir, int yDir)
        where T : Component //泛型函数T用于指定进行碰撞交互的单位的组建类型,使用关键字“where”说明“T”将为一个组件，
    {
        RaycastHit2D hit;
        bool canMove = Move(xDir, yDir, out hit);   //可以移动则为true，反之为false
        //hit是Move的输出参数，它可以让我们检测“hit.transform”是否等于“null”，如果射线没有碰到任何东西直接返回，那么不执行下面的代码
        if (hit.transform == null)
            return;

        T hitComponent = hit.transform.GetComponent<T>();

        if (!canMove && hitComponent != null)
            OnCantMove(hitComponent);
    }

    protected IEnumerator SmoothMovement(Vector3 end)   //把物体移动到下一个位置，end为结束位置
    {
        //remaining:逗留,剩下           sqrMagnitude 计算长度的平凡的函数
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(rd2D.position, end, inversMoveTime * Time.deltaTime);       //Vector3.MoveTowards是把一个点直线移动到目标点
            rd2D.MovePosition(newPosition);         //移动到新点
            sqrRemainingDistance = (transform.position - end).sqrMagnitude; //重新计算距离
            yield return null;      //等待一秒再判断是否满足条件
        }
    }

    //abstract表示这个函数不完整，在派生类中将被重写
    protected abstract void OnCantMove<T>(T component)
        where T : Component;        //泛型函数T用于指定进行碰撞交互的单位的组建类型

}
