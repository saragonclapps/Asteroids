using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FactoryPool : MonoBehaviour {
    
    public Pool<Explotion> explotionBulletPool;
    public Pool<Explotion> explotionAsteroidPool;
    public Pool<BulletBasic> basicBulletsPool;

    private string _pathBulletBasic = "Bullets/BulletBasic";
    private string _pathExplotionBullet = "VFX/ExplotionBullet";
    private string _pathExplotionAsteroid = "VFX/ExplotionAsteroid";

    public static FactoryPool instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else
        {
            instance = null;
            instance = this;
        }
    }

    private void Start()
    {
        basicBulletsPool = new Pool<BulletBasic>(10, CreateBullets, BulletBasic.Initialize, BulletBasic.Dispose, true);
        explotionBulletPool = new Pool<Explotion>(5, CreateExplotionBullet, Explotion.Initialize, Explotion.Dispose, true);
        explotionAsteroidPool = new Pool<Explotion>(5, CreateExplotionAsteroid, Explotion.Initialize, Explotion.Dispose, true);
    }

    private BulletBasic CreateBullets()
    {
        var prefab = Instantiate(Resources.Load(_pathBulletBasic, typeof(BulletBasic))) as BulletBasic;
        return prefab;
    }

    private Explotion CreateExplotionBullet()
    {
        var prefab = Instantiate(Resources.Load(_pathExplotionBullet, typeof(Explotion))) as Explotion;
        return prefab;
    }

    private Explotion CreateExplotionAsteroid()
    {
        var prefab = Instantiate(Resources.Load(_pathExplotionAsteroid, typeof(Explotion))) as Explotion;
        return prefab;
    }

    public void ReturnBulletsBasicsToPool(BulletBasic asteroid)
    {
        basicBulletsPool.DisablePoolObject(asteroid);
    }

    public void ReturnExplotionBulletToPool(Explotion explotion)
    {
        explotionBulletPool.DisablePoolObject(explotion);
    }

    public void ReturnExplotionAsteroidToPool(Explotion explotion)
    {
        explotionBulletPool.DisablePoolObject(explotion);
    }
}
