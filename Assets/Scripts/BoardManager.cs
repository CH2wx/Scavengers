/// <summary>
/// 文档作用：
/// 作者：陈鸿
/// 编辑时间：
/// 备注：实例化游戏场景
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;                               //为了使用Serializable
using System.Collections.Generic;           //为了使用List
using Random = UnityEngine.Random;          //使两个随即函数不冲突

public class BoardManager : MonoBehaviour
{

    [Serializable]
    public class Count
    {
        public int mininum;
        public int maxinum;

        public Count(int min, int max)
        {
            mininum = min;
            maxinum = max;
        }
    }

    //行和列的数量
    public int columns = 8;
    public int rows = 8;

    public Count foodCount = new Count(1, 5);       //每个关卡中食物的数量
    public Count wallCount = new Count(5, 9);       //每个关卡中内墙的数量
    public GameObject exits;
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles;
    public GameObject[] outerWallTiles;

    private Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();

    void InitaliseList()
    {
        gridPositions.Clear();
        for (int x = 1; x < columns - 1; x++)
        {
            for (int y = 1; y < rows - 1; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));//留出的外圈是为了防止关卡无解设计的
            }
        }
    }

    //用于生成外墙和地面
    void SetBoard()
    {
        boardHolder = new GameObject("Board").transform;

        for (int x = -1; x < columns + 1; x++)
        {
            for (int y = -1; y < rows + 1; y++)
            {
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];

                if (x == -1 || y == -1 || x == columns || y == rows)
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];

                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0), Quaternion.identity) as GameObject;

                instance.transform.SetParent(boardHolder);
            }
        }
    }

    Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }

    void LayoutObjectsRandom(GameObject[] tileArray, int mininum, int maxinum)
    {
        int objectCount = Random.Range(mininum, maxinum + 1);

        GameObject instantiateParent = GameObject.Find("Empty").gameObject;

        for (int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            GameObject go = Instantiate(tileChoice, randomPosition, Quaternion.identity) as GameObject;
            go.transform.parent = instantiateParent.transform;
        }
    }

    //新建场景
    public void SetupScene(int level)
    {
        SetBoard();                                                                         //设置地面和墙
        InitaliseList();                                                                    //初始化可以使用的地面
        LayoutObjectsRandom(wallTiles, wallCount.mininum, wallCount.maxinum);               //实例化墙
        LayoutObjectsRandom(foodTiles, foodCount.mininum, foodCount.maxinum);               //实例化食物
        int enemyCount = (int)Mathf.Log(level, 2f);                                         //取以2为底level的对数    
        LayoutObjectsRandom(enemyTiles, enemyCount, enemyCount);                            //实例化敌人
        Instantiate(exits, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);    //实例化出口
    }

}
