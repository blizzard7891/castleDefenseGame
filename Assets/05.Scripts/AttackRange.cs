using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour {
  
    public Queue<Monster> monsters = new Queue<Monster>();
    public List<Monster> targets = new List<Monster>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Monster")
        {
            monsters.Enqueue(collision.GetComponent<Monster>());
            targets.Add(monsters.Dequeue());
        }
    }
}
