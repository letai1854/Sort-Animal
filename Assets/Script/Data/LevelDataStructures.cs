using System.Collections.Generic;
using UnityEngine;

public class LevelConfig
{
    public int level;
    public string comment;
    public int tubeHeight;
    public List<TubeData> tubes;
}

public class TubeData
{
    public List<string> animals;
}

public class AllLevelsData
{
    public List<LevelConfig> levels;
}