using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnAutoPhysicsScene : MonoBehaviour
{
    public string autoPhysicsScene;
    public PhysicsScene physicsScene;
    public GameObject objects;

    void Start()
    {  
        var scene = SceneManager.CreateScene(autoPhysicsScene, new CreateSceneParameters( LocalPhysicsMode.Physics3D ));
        //var scene = SceneManager.GetSceneByName(autoPhysicsScene);
        physicsScene = scene.GetPhysicsScene();

        var obj = Instantiate(objects, transform);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.SetParent(null);

        SceneManager.MoveGameObjectToScene(obj, scene);

    }

    void FixedUpdate()
    {
        physicsScene.Simulate(Time.fixedDeltaTime);
        //physicsScene.Simulate(Time.fixedDeltaTime / 2);
    }

    [ContextMenu("")]
    void UpdateToIdle()
    {

    }
}
