using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookUpTable<Src, Dst>{

    public delegate Dst FactoryMethod(Src newValue);
    private FactoryMethod _factoryMethod;

    private Dictionary<Src, Dst> _table = new Dictionary<Src, Dst>();

    public LookUpTable(FactoryMethod factoryMethod)
    {
        _factoryMethod = factoryMethod;
    }

    public Dst GetValue(Src key)
    {
        if (!_table.ContainsKey(key)) _table[key] = _factoryMethod(key);
        return _table[key];
    }
}
