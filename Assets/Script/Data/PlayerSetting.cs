using UnityEngine;

public enum BackgroundType
{
    Forest,
    Ocean,
    Desert
}



[System.Serializable]
public class PlayerSettings
{
    public string coins;
    public BackgroundType background;
    public AnimalHabitatType habitat;
}