using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnAutoPhysicsScene2D : MonoBehaviour
{
    public string autoPhysicsScene;
    public PhysicsScene2D physicsScene;
    public GameObject objects;

    private GameObject obj;
    public bool enableSim;

    void Start()
    {  
        var scene = SceneManager.CreateScene(autoPhysicsScene, 
            new CreateSceneParameters( LocalPhysicsMode.Physics2D )); 
        physicsScene = scene.GetPhysicsScene2D();
        obj = Instantiate(objects, transform);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.SetParent(null);

        SceneManager.MoveGameObjectToScene(obj, scene);
    }

    void FixedUpdate()
    {
        //physicsScene.Simulate(Time.fixedDeltaTime);
        if(enableSim) physicsScene.Simulate(Time.fixedDeltaTime);
    }

    [ContextMenu("Shoot and Update 1000")]
    public void ShootAndUpdate1000()
    {
        Shoot();
        Iteration(1000);
    }

    [ContextMenu("Shoot")]
    public void Shoot()
    { 
        obj.GetComponent<Objects2D>().Hit();
    }

    [ContextMenu("Update 1000 Iteration")]
    public void Iteration1000()
    {
        Iteration(1000);
    }

    public void Iteration(int n)
    {
        for (int i = 0; i < n; i++)
        {
            physicsScene.Simulate(Time.fixedDeltaTime);

        }
    }
}
