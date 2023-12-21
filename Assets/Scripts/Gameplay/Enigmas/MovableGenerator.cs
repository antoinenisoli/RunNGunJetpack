using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableGenerator : MonoBehaviour
{
    [SerializeField] GameObject[] movables;
    [SerializeField] Vector2Int randomAmount = new Vector2Int(10, 15);

    private void Awake()
    {
        int random = Random.Range(randomAmount.x, randomAmount.y);
        for (int i = 0; i < random; i++)
        {
            int movableRandom = Random.Range(0, movables.Length);
            GameObject m = Instantiate(movables[movableRandom], transform.position, Quaternion.identity);  
        }
    }
}
