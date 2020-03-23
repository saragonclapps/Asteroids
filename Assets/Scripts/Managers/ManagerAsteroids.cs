using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Object = UnityEngine.Object;

public class ManagerAsteroids : MonoBehaviour
{
    //Directions of 360 degress.(Destroy asteroids)
    private List<Vector3> directionsDestroyAsteroid = new List<Vector3>();

    //Look at table pattern.
    private LookUpTable<Vector3, Vector3> directions;
    private LookUpTable<int, Object> loadAsteroids;

    //Control time.
    public float defaultTimeValue { get; private set; }
    public float timeLoadNextAsteroid { get; private set; }

    //Pool pattern.
    public Pool<MainAsteroid> mainAsteroidPool;
    public Pool<MiniAsteroid> miniAsteroidPool;
    private const string _pathAsteroid = "Asteroids/Asteroid ";

    //Coroutine.
    private CoroutineUpdate _coroutineUpdate;

    public static ManagerAsteroids instance;

    private float _totalAsteroids = 0;
    public float GetTotalAsteroids { get { return _totalAsteroids; } }

    private void Awake()
    {
        //Singleton.
        if (instance != null)
        {
            Destroy(this);
            instance = this;
        }
        else
        {
            instance = this;
        }
        //Creation of the lookup tables.
        loadAsteroids = new LookUpTable<int, Object>(ReourcesMethod);
        directions = new LookUpTable<Vector3, Vector3>(DirectionMethod);

        //Load all values in the lookup table ResorcesMethod.
        for (int i = 1; i < 9; i++)
           loadAsteroids.GetValue(i);

        //Creating the firts asteroids.
        mainAsteroidPool = new Pool<MainAsteroid>(10, CreateAsteroid, MainAsteroid.Initialize, MainAsteroid.Dispose, true);
        miniAsteroidPool = new Pool<MiniAsteroid>(10, CreateMiniAsteroid, MiniAsteroid.Initialize, MiniAsteroid.Dispose, true);
    }

    private void Start()
    {
        timeLoadNextAsteroid = defaultTimeValue = 6;
        LoadDirection();

        //Creation coroutine.
        _coroutineUpdate = new CoroutineUpdate(true, timeLoadNextAsteroid);

        _coroutineUpdate.Subscribe(() => {
            var asteroid = mainAsteroidPool.GetObjectFromPool();
            var spawnPosition = SpawnPosition();
            asteroid.transform.position = spawnPosition;
            asteroid.GetComponent<MainAsteroid>().direction = directions.GetValue(spawnPosition);

            _coroutineUpdate.time = timeLoadNextAsteroid;
        });

        StartCoroutine(_coroutineUpdate.CoroutineMethod());
    }

    public void NotifyDiableAsteroid()
    {
        _totalAsteroids--;
    }

    public void NotifyEnableAsteroid()
    {
        _totalAsteroids++;
    }

    private Object ReourcesMethod(int value = 1){
        return Resources.Load(_pathAsteroid + value);
    }

    private Vector3 DirectionMethod(Vector3 newValue){
        return new Vector3(PointsReferences.instance.GetCenter.x - newValue.x,PointsReferences.instance.GetCenter.y - newValue.y).normalized;
    }

    /// <summary>
    /// "Procedural" spawn time.
    /// </summary>
    public void LoadNextValueTime()
    {
        timeLoadNextAsteroid -= (defaultTimeValue >= 1) ? timeLoadNextAsteroid * 0.1f : 0;
    }

    /// <summary>
    /// Restart attributes.
    /// </summary>
    public void ResetValues()
    {
        timeLoadNextAsteroid = defaultTimeValue;
    }

    private Vector3 SpawnPosition()
    {
        var spawnPosition = Vector3.zero;

        if (Random.Range(0, 2) >= 1){
            spawnPosition.x = Random.Range(PointsReferences.instance.GetLeft, PointsReferences.instance.GetRight);
            spawnPosition.y = (Random.Range(0, 2) >= 1) ? PointsReferences.instance.GetUp : PointsReferences.instance.GetDown;
        }else{
            spawnPosition.x = (Random.Range(0, 2) >= 1) ? PointsReferences.instance.GetLeft : PointsReferences.instance.GetRight;
            spawnPosition.y = Random.Range(PointsReferences.instance.GetUp, PointsReferences.instance.GetDown);
        }
        return spawnPosition;
    }

    public void SpawnToDestroyAsteroid( Vector3 positionDestroy, int take = 3 )
    {
        List<Vector3> takeDirections = new List<Vector3>(directionsDestroyAsteroid);
        for (int i = 0; i < take; i++)
        {
            var itemRandom = takeDirections[Random.Range(0, takeDirections.Count - 1)];
            var asteroid = miniAsteroidPool.GetObjectFromPool();
            asteroid.transform.position = positionDestroy + itemRandom;
            asteroid.direction = itemRandom;
            takeDirections.Remove(itemRandom);
        }
    }

    private void LoadDirection()
    {
        directionsDestroyAsteroid.Add(new Vector3(0, 1, 0));        // Up
        directionsDestroyAsteroid.Add(new Vector3(1, 0, 0));        // right
        directionsDestroyAsteroid.Add(new Vector3(0, -1, 0));       // Down
        directionsDestroyAsteroid.Add(new Vector3(-1, 0, 0));       // Left

        directionsDestroyAsteroid.Add(new Vector3(0.5f, 0.5f, 0));  // Up Right
        directionsDestroyAsteroid.Add(new Vector3(0.5f, -0.5f, 0)); // Down right
        directionsDestroyAsteroid.Add(new Vector3(-0.5f, -0.5f, 0));// Down Left
        directionsDestroyAsteroid.Add(new Vector3(-0.5f, 0.5f, 0)); // Up Left
    }

    #region pool

    public MainAsteroid CreateAsteroid()
    {
        var prefab = Instantiate(loadAsteroids.GetValue(Random.Range(1, 9)) as GameObject);
        prefab.transform.SetParent(transform);
        prefab.AddComponent<MainAsteroid>();
        return prefab.GetComponent<MainAsteroid>();
    }

    public MiniAsteroid CreateMiniAsteroid()
    {
        var prefab = Instantiate(loadAsteroids.GetValue(Random.Range(1, 9)) as GameObject);
        prefab.transform.SetParent(transform);
        prefab.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        prefab.AddComponent<MiniAsteroid>();
        return prefab.GetComponent<MiniAsteroid>();
    }

    public void ReturnAsteroidToPool(MainAsteroid asteroid)
    {
        mainAsteroidPool.DisablePoolObject(asteroid);
    }

    public void ReturnMiniAsteroidToPool(MiniAsteroid asteroid)
    {
        miniAsteroidPool.DisablePoolObject(asteroid);
    }

    #endregion pool.

    private void OnDestroy()
    {
        _coroutineUpdate.Clean();
        _coroutineUpdate = null;
    }
}