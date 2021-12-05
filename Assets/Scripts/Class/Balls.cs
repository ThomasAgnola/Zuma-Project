using UnityEngine;
using System.Collections;
using System;

public class Balls : IComparable<Balls>
{
    public string name;
    public int index;

    public Balls(string newName, int newindex)
    {
        name = newName;
        index = newindex;
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
}
