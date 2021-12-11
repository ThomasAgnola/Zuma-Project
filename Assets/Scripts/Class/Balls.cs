using UnityEngine;
using System.Collections;
using System;

public class Balls : IComparable<Balls>
{
    public string color;
    public int index;
    public GameObject go;

    public Balls(string newcolor, int newindex, GameObject newgo)
    {
        color = newcolor;
        index = newindex;
        go = newgo;
    }

    //This method is required by the IComparable
    //interface. 
    public int CompareTo(Balls other)
    {
        if (other == null)
        {
            return 1;
        }

        //Return the difference in power.
        return index - other.index;
    }

    public int GetIndex(Balls ball)
    {
        return ball.index;
    }

    public string GetColor(Balls ball)
    {
        return ball.color;
    }
}
