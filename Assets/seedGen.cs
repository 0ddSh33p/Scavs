using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class seedGen : MonoBehaviour
{
    public Vector2 seed;
    void Awake()
    {
        seed = new Vector2(Random.Range(-2000,2000),Random.Range(-2000, 2000));
    }
}
