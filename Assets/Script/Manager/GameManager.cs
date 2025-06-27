using System.Collections.Generic;
using UnityEngine;





public class GameManager : SingletonMonoBehaviour<GameManager>
{


    public Canvas mainUICanvas;
    public PlayerSettings playerSettings;
    private PlayerDataService playerDataService;


    public List<DataItem> habitatAnimal;
    public List<DataItem> shopBGDataList;

    private void Awake()
    {
        playerDataService = new PlayerDataService();
        playerSettings = playerDataService.LoadPlayerSettings(CONST.SETTINGLOAD);

    }
    void Start()
    {
        UIManager.Instance.ShowScreen<HomeScreen>();
        
    }

    void Update()
    {
        
    }
}
