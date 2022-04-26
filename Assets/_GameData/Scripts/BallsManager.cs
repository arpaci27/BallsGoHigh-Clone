using System.Collections;
using System.Collections.Generic;
using Unity.EditorCoroutines.Editor;
using UnityEngine;

public class BallsManager : MonoSingleton<BallsManager>
{
    public List<GameObject> balls = new List<GameObject>();
    
    public GameObject ballPrefab;
    
    public Transform ballsParent;

    public int maxSortingLayer = 4;

    [Space] [Range(0, 300)] 
    public int ballCount;

    private bool _isDecrease;
    

    private void OnValidate()
    {
        if (ballCount>balls.Count && !_isDecrease)
        {
            CreateBalls();
        }
        else if (balls.Count > ballCount)
        {
            _isDecrease = true;
            
            DestroyBalls();
        }
        else
        {
            Sort();
        }
    }

    #region OnValidate Functions

    private void CreateBalls()
    {
        for (var var = balls.Count; var < ballCount; var++)
        {
            var go = Instantiate(ballPrefab, ballsParent);
            
            balls.Add(go);

            go.transform.localPosition = Vector3.zero;
        }
        Sort();
    }

    private void DestroyBalls()
    {
        for (var i = balls.Count-1; i >= ballCount; i--)
        {
            var go = balls[balls.Count - 1];
            
            balls.RemoveAt(i);

            EditorCoroutineUtility.StartCoroutine(DestroyObject(go), this);
        }

        _isDecrease = false;
        Sort();
    }

    #endregion

    
    private static void MoveObjects(Transform objectTransform, float degree , float dist)
    {
        var pos = Vector3.zero;
        
        pos.x = Mathf.Cos(degree * Mathf.Deg2Rad);
        pos.z = Mathf.Sin(degree * Mathf.Deg2Rad);

        objectTransform.localPosition = pos * dist;
    }

    private void Sort()
    {
        var ballsCount = balls.Count;

        var num = 0;

        for (var i = 0; i < maxSortingLayer; i++)
        {
            if (ballsCount == num)
                break;

            var innerCount = i * 6 == 0 ? 1 : i * 6;
            var angle = 360 / innerCount;

            for (var j = 0; j < innerCount; j++)
            {
                if (ballsCount == num)
                    break;
                
                MoveObjects(balls[num].transform, j * angle,i);
                num++;
            }
        }
    }

    private new static IEnumerator DestroyObject(Object go)
    {
        yield return new WaitForEndOfFrame();
        DestroyImmediate(go);
    }

    public void AddBalls(int addingNum)
    {
        ballCount += addingNum;

        for (var i = 0; i < addingNum; i++)
        {
            var go = Instantiate(ballPrefab, ballsParent);
            
            balls.Add(go);

            go.transform.localPosition = Vector3.zero;
        }
        
        Sort();
    }
    
    public void RemoveBalls(int removingNum)
    {
        for (var i = ballCount-1; i >= ballCount-removingNum; i--)
        {
            var go = balls[balls.Count - 1];
            
            balls.RemoveAt(i);

            EditorCoroutineUtility.StartCoroutine(DestroyObject(go), this);
        }

        ballCount = balls.Count;
        Sort();
    }
}
