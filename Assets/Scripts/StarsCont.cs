using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarsCont : MonoBehaviour {
    public static StarsCont SharedInstance;
    public List<ClonePath> stars = new List<ClonePath>();
    public List<Person> people = new List<Person>();

    private void Awake()
    {
        SharedInstance = this;
    }

    private void Update()
    {
        if(stars.Count > 0)
        {
            for (int i = 0; i < stars.Count; i++)
            {
                stars[i].Move();
            }
        }

        if (people.Count > 0)
        {
            for (int i = 0; i < people.Count; i++)
            {
                people[i].Move();
            }
        }
    }
}
