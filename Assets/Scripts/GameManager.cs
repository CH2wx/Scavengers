/// <summary>
/// 文档作用：
/// 作者：陈鸿
/// 编辑时间：
/// 备注：加载关卡，管理分数
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public float levelStartDelay = 2f;  //游戏开始前的等待时间
    public float turnDelay = .1f;    //这是移动的间隔时间
    public int playerFoodPoints = 100;

    public static GameManager instance = null;  //public是为了可以从外部进行访问，static表示它属于这个类的本身并且作为这个类的实例使用
    [HideInInspector] public bool playersTurn = true;

    private BoardManager boardScript;
    private GameObject levelImage;
    private GameObject ButtonReset;
    private GameObject root;        //设置节点，用于查找到被隐藏的对象
    private Text levelText; //用于文本显示当前关卡序号，初始值设为"Day 1"
    private int level = 1;      //表示第二关开始才有敌人
    private bool doingSetup;    //用于检测场景是否加载完毕，防止人物的加载前移动

    private List<Enemy> enemies;
    private bool enemiesMoving;

    public void OnLevelWasLoaded(int index)
    {
        level++;
        InitGame();
    }

    // Use this for initialization
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);        //确保只有一个GameManager

        DontDestroyOnLoad(gameObject);  //防止在切换场景时脚本背销毁
        enemies = new List<Enemy>();
        boardScript = GetComponent<BoardManager>();


        InitGame();
    }

    public void GameOver()
    {
        levelText.text = "之后，你经过了" + level + "天，饿死了...";
        levelImage.SetActive(true);
        ButtonReset.SetActive(true);
        enabled = false;
    }

    void InitGame()
    {
        doingSetup = true;
        
        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Day" + level;
        levelImage.SetActive(true);
        
        root = GameObject.Find("Canvas");
        ButtonReset = root.transform.Find("Button_Reset").gameObject;
        ButtonReset.SetActive(false);

        Invoke("HideLevelImage", levelStartDelay);  //标题会显示两秒后才消失,调用了HideLevelImage函数
        
        enemies.Clear();                            //让GameManager在关卡开始时不会被重置，而我们清空上一关的敌人
        boardScript.SetupScene(level);              //新建场景，并实例化各种参数
    }
    
    //隐藏关卡的背景图片
    private void HideLevelImage()
    {
        levelImage.SetActive(false);                //禁用背景图片的渲染
        doingSetup = false;
    }

    void Update()
    {
        if (playersTurn || enemiesMoving)  //如果敌人有移动
            return;
        StartCoroutine(MoveEnemies());
    }

    public void AddEnemyToList(Enemy script)    //作用用于敌人在GameManager中注册，从而可以接收移动指令
    {
        enemies.Add(script);
    }

    IEnumerator MoveEnemies()       //控制敌人移动的协同函数
    {
        enemiesMoving = true;
        yield return new WaitForSeconds(turnDelay);

        if(enemies.Count==0)    //如果没有敌人，则玩家等待
        {
            yield return new WaitForSeconds(turnDelay);
        }

        for(int i=0;i<enemies.Count;i++)
        {
            enemies[i].MoveEnemy();
            yield return new WaitForSeconds(enemies[i].moveTime);
        }

        playersTurn = true;
        enemiesMoving = false;
    }

    public void Reset()
    {
        level = 1;
        playerFoodPoints = 100;
        
        InitGame();
        enabled = true;
        enemies.Clear();
    }

    public bool ReturnDoingSetup()
    {
        return doingSetup;
    }
}
