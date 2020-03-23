using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Operations{

    public LookUpTable<float, float> lookUpTableSin;
    public LookUpTable<float, float> lookUpTableCos;
    
    public Operations()
    {
        lookUpTableCos = new LookUpTable<float, float>(x => Mathf.Sin(x));
        lookUpTableCos = new LookUpTable<float, float>(y => Mathf.Cos(y));
    }
}
