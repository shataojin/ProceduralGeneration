using UnityEngine;

public class MainManager : MonoBehaviour
{
    void Start()
    {
        ProceduralDungeonGenerator.Init();
        DungeonVisuals.Init();
        ProceduralDungeonGenerator.ProcedurallyGenerateDungeon();
        DungeonVisuals.CreateVisualsFromModelData();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ProceduralDungeonGenerator.DestroyDungeon();
            DungeonVisuals.DestroyDungeonVisuals();
            ProceduralDungeonGenerator.ProcedurallyGenerateDungeon();
            DungeonVisuals.CreateVisualsFromModelData();
        }
    }
}
