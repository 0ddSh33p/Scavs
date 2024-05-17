using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class seedGen : MonoBehaviour
{
    public Vector2 seed;
    public List<GameObject> players;
    
    void Awake()
    {
        seed = new Vector2(Random.Range(-99999,99999),Random.Range(-99999, 99999));
    }
}
