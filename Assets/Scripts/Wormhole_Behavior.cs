using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wormhole_Behavior : MonoBehaviour
{
    public GameObject target;
    public float rotation = 50f;

    private List<GameObject> travellers;
    // Start is called before the first frame update
    void Start()
    {
        travellers = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * rotation * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject);
        if (target != null && !travellers.Contains(collision.gameObject))
        {
            var tar = target.GetComponent<Wormhole_Behavior>();
            if (tar)
            {
                tar.travellers.Add(collision.gameObject);
            }
            collision.transform.position = target.transform.position;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (travellers.Contains(collision.gameObject))
        {
            travellers.Remove(collision.gameObject);
        }
    }
}
