/// <summary>
/// 文档作用：
/// 作者：陈鸿
/// 编辑时间：
/// 备注：控制角色
/// </summary>
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MovinObject {

    public int wallDamage = 1;  //玩家的伤害为1
    public int pointPerFood = 10;   //食物增加的点数，下同
    public int pointPerSoda = 20;
    public float restarLevelDelay = 1f;     //加载关卡的时间

    public Text foodText;

    private Animator animator;
    private int food;           //储存关卡当前得分

    public AudioClip moveSound1;
    public AudioClip moveSound2;
    public AudioClip eatSound1;
    public AudioClip eatSound2;
    public AudioClip drinkSound1;
    public AudioClip drinkSound2;
    public AudioClip gameOverSound;

    private Vector2 touchOrigin = -Vector2.one;

    // 重写父类需要用到protected override
    protected override void Start()
    {
        animator = GetComponent<Animator>();

        food = GameManager.instance.playerFoodPoints;   //实时更新食物点数，在切换关卡时再传递给GameManager

        foodText.text = "Food:" + food;

        base.Start();
    }

    //另食物值等于玩家的生命值
    private void OnDisable()
    {
        GameManager.instance.playerFoodPoints = food;
        food = GameManager.instance.playerFoodPoints;
    }

    void Update ()
    {
        if (!GameManager.instance.playersTurn)
            return;

        int horizontal = 0;
        int vertical = 0;

    #if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPALYER

        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");
        //print("horizontal=" + horizontal + "\nvertical=" + vertical);

        if (horizontal != 0)
        {
            vertical = 0;   //防止斜向移动
        }
    #elif UUINTY_IOS || UNITY_ANDROID || UNITY_WPB || UNITY_IPHONE

        if(Input.touchCount>0)
        {
            Touch myTouch = Input.touches[0];   //取按下的第一个点，忽略后面按下的点

            if(myTouch.phase==TouchPhase.Began) //判断是否为触摸的起始点
            {
                touchOrigin = myTouch.position;
            }

            else if(myTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0 )  //TouchOrigin的初始值为-1，这里检测如果触摸动作结束，并且触摸起始点的X坐标大于等于0，意味着触摸点在屏幕范围内，“touchOrigin”值也发生了改变，且触摸已结束
            {
                Vector2 touchEnd = myTouch.position;

                float x = touchEnd.x - touchOrigin.x;   //可以获取X轴滑屏方向
                float y = touchEnd.y - touchOrigin.y;
                touchOrigin.x = -1;
                if (Mathf.Abs(x) > Mathf.Abs(y))
                    horizontal = x > 0 ? 1 : -1;
                else
                    vertical = y > 0 ? 1 : -1;
            }
        }

        #endif

        if (horizontal != 0 || vertical != 0)
            AttemptMove<Wall>(horizontal, vertical);    //如果玩家碰到墙
	}

    protected override void OnCantMove<T>(T component)
    {
        Wall hitWall = component as Wall;

        hitWall.DamageWall(wallDamage);
        animator.SetTrigger("PlayerChop");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Exit")
        {
            Invoke("Restart", restarLevelDelay);
            enabled = false;
        }
        else if (other.tag == "Food")
        {
            food += pointPerFood;
            foodText.text = "+" + pointPerFood + "Food:" + food;
            SoundManager.instance.RandomizeSfx(eatSound1, eatSound2);
            other.gameObject.SetActive(false);
        }
        else if(other.tag=="Soda")
        {
            food += pointPerSoda;
            foodText.text = "+" + pointPerSoda + "Food:" + food;
            SoundManager.instance.RandomizeSfx(drinkSound1, drinkSound2);
            other.gameObject.SetActive(false);
        }
        OnDisable();
    }

    private void Restart()
    {
        SceneManager.LoadScene(0);                  //加载最后一次加载的场景
    }

    public void LoseFood(int loss)
    {
        animator.SetTrigger("PlayerHit");
        food -= loss;
        foodText.text = "-" + loss + "food" + food;
        CheckIfGameOver();
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        food--;
        foodText.text = "Food:" + food;

        base.AttemptMove<T>(xDir, yDir);

        RaycastHit2D hit;
        if(Move(xDir,yDir,out hit))
        {
            SoundManager.instance.RandomizeSfx(moveSound1, moveSound2);
        }

        CheckIfGameOver();

        GameManager.instance.playersTurn = false;       //表示玩家已经移动过
    }

    private void CheckIfGameOver()
    {
        if (food <= 0)
        {
            SoundManager.instance.PlaySingle(gameOverSound);
            SoundManager.instance.musicSource.Stop();
            GameManager.instance.GameOver();

            this.gameObject.transform.position = new Vector3(0, 0, 0);
        }
    }

    public void HitResetButton()
    {
        DestoryObject.instance.DestroyPrtAndSon();
        food = 100;
        OnDisable();
        GameManager.instance.Reset();
        SoundManager.instance.efxSource.Play();
        SoundManager.instance.musicSource.Play();
        foodText.text = "Food:" + food;
    }
}
