/*

 Description:        Simple example of an Object Pool, used specifically for obstacles in one of my projects
                     Already generic except for naming conventions, could be improved by adding variable/adaptive sizes if need be

David Griffith 2017
 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObstaclePool : MonoBehaviour
{
    public int size = 50;

    public GameObject obstacle;

    private List<GameObject> obstacleList;
    public List<GameObject> ObstacleList
    {
        get
        {
            return obstacleList;
        }
    }


    void Start()
    {
        InitializePool();
    }

    public void InitializePool()
    {
        obstacleList = new List<GameObject>(size);

        for (int i = 0; i < size; i++)
        {
            GameObject obstacleObject = Instantiate(obstacle);
            obstacleObject.transform.parent = transform;

            obstacleObject.GetComponent<Obstacle>().Init();

            obstacleList.Add(obstacleObject);
        }
    }

    public void ClearPool()
    {
        for (int i = obstacleList.Count - 1; i > 0; i--)
        {
            GameObject obstacleObject = obstacleList[i];
            obstacleList.RemoveAt(i);
            Destroy(obstacleObject);
        }

        obstacleList = null;
    }

    public GameObject GetObstacle()
    {
        if (obstacleList.Count > 0)
        {
            GameObject obstacleObject = obstacleList[0];
            obstacleList.Remove(obstacleObject);

            return obstacleObject;
        }

        return null;
    }

    public void DestroyObstacle(GameObject obstacleObject)
    {
        obstacleObject.SetActive(false);
        obstacleList.Add(obstacleObject);
    }
}
