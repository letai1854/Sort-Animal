using JetBrains.Annotations;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;










public class GameModle
{
    private System.Random randomGenerator;

    public List<TubeModle> tubes { get; private set; }
   public int totalTubeCount { get; private set; }
    public int tubeHeight;
    public int numberOfEmptyTubes { get; private set; }
    public  int numberOfAnimalTypes { get; private set; }
    public int level;

    public class HintMove
    {
        public int SourceTubeIndex { get; set; }
        public int DestinationTubeIndex { get; set; }
        public bool IsValid { get; set; } 

        public HintMove() { IsValid = false; } 
    }

    private List<AnimalType> activeAnimalTypes;

    //public GameModle (int tubeCount, int tubeHeight, int tubeEmpty)
    //{

    //    randomGenerator = new System.Random ();
    //    tubes = new List<TubeModle> ();

    //    this.totalTubeCount = tubeCount;
    //    this.numberOfEmptyTubes = tubeEmpty;
    //    this.tubeHeight = tubeHeight;
    //    numberOfAnimalTypes = tubeCount -tubeEmpty;
    //    int maxAnimalTypes = System.Enum.GetValues(typeof(AnimalType)).Length;

    //    if (numberOfAnimalTypes > maxAnimalTypes)
    //    {
    //        numberOfAnimalTypes = maxAnimalTypes;
    //    }
    //    for (int i = 0; i < totalTubeCount; i++)
    //    {
    //        tubes.Add(new TubeModle(tubeHeight));
    //    }

    //    AnimalType[] allPossibleTypes = (AnimalType[])System.Enum.GetValues(typeof(AnimalType));
    //    if (allPossibleTypes.Length > maxAnimalTypes)
    //    {
    //        throw new System.ArgumentException($"Not enough unique AnimalTypes defined in enum ({allPossibleTypes.Length}) to support different types needed for the level.");
    //    }
    //    activeAnimalTypes = allPossibleTypes.OrderBy(x => randomGenerator.Next()).Take(this.numberOfAnimalTypes).ToList();
    //    GenerateSolveableLevel();
    //}
    private void GenerateSolveableLevel()
    {
        for (int indexType = 0; indexType < this.numberOfAnimalTypes; indexType++)
        {
            AnimalType currentAnimalType = activeAnimalTypes[indexType];
            TubeModle targetTube = tubes[indexType];
            for (int indexItem = 0; indexItem < this.tubeHeight; indexItem++) 
            { 
                targetTube.AddAnimal(currentAnimalType);
            }
        }
        
        int totalAnimalsPackTubes = this.numberOfAnimalTypes * this.tubeHeight;
        int minMoves = totalAnimalsPackTubes * CONST.MIN_SHUFFLE_MOVES_PER_ANIMAL;
        int maxMoves = totalAnimalsPackTubes*CONST.MAX_SHUFFLE_MOVES_PER_ANIMAL + 1;
        int shuffleMovesCount  = randomGenerator.Next(minMoves, maxMoves);

        for (int i = 0; i < shuffleMovesCount; i++) 
        {
            List<int> possibleSourceIndices = Enumerable.Range(0, tubes.Count)
                                            .Where(i => !tubes[i].IsEmpty())
                                            .ToList();
            if (possibleSourceIndices.Count == 0) continue;
            int sourceTubesIndex = possibleSourceIndices[randomGenerator.Next(possibleSourceIndices.Count)];

            List<int> possibleTargetIndices = Enumerable.Range(0, tubes.Count)
                                                        .Where(i => i != sourceTubesIndex && !tubes[i].IsFull())
                                                        .ToList();
            if (possibleTargetIndices.Count == 0) continue; 
            int targetTubesIndex = possibleTargetIndices[randomGenerator.Next(possibleTargetIndices.Count)];

            TubeModle sourceTube = tubes[sourceTubesIndex];
            TubeModle targetTube = tubes[targetTubesIndex];
         
            if(!sourceTube.IsEmpty() && !targetTube.IsFull())
            {
                AnimalType? animalSource = sourceTube.Pop();
                if (animalSource.HasValue)
                {
                    targetTube.AddAnimal(animalSource.Value);
                }
            }
        }
    }
    
    public GameModle ()
    {
        //tubes = new List<TubeModle>();
        //TextAsset jsonFile = Resources.Load<TextAsset>(CONST.LEVEL_DATA_FILE_PATH);
        //if (jsonFile == null)
        //{
        //    Debug.LogError($"Could not load level data file: Resources/.json (or without extension)");
        //}
        //string jsonData = jsonFile.text;
        //LevelConfig targetLevelConfig = null;
        //AllLevelsData allLevelsData = JsonConvert.DeserializeObject<AllLevelsData>(jsonData);
        //targetLevelConfig = allLevelsData.levels.FirstOrDefault(lvl => lvl.level == levelIdToLoad);
        //this.totalTubeCount = targetLevelConfig.tubes.Count;
        //this.tubeHeight = targetLevelConfig.tubeHeight;
        //this.tubes.Clear();
        //for (int i = 0; i < totalTubeCount; i++)
        //{
        //    tubes.Add(new TubeModle(targetLevelConfig.tubeHeight));
        //}
        //for (int i =0; i< totalTubeCount; i++)
        //{
        //    TubeData tubeDataFromJson = targetLevelConfig.tubes[i];

        //    foreach (string animalNameString in tubeDataFromJson.animals)
        //    {
        //        if (System.Enum.TryParse<AnimalType>(animalNameString, true, out AnimalType parsedAnimalType)) 
        //        {
        //            tubes[i].animals.Add(parsedAnimalType);
        //        }

        //    }
        //}
      
     
    }
    
    public void NewGame()
    {
        tubes = new List<TubeModle>();
        LevelConfig targetLevelConfig = null;
        targetLevelConfig = LevelDataLoader.Instance.GetAllLevelsData(CONST.LEVEL_DATA_FILE_PATH);
        this.totalTubeCount = targetLevelConfig.tubes.Count;
        this.tubeHeight = targetLevelConfig.tubeHeight;
        this.level = targetLevelConfig.level;
        this.tubes.Clear();
        for (int i = 0; i < totalTubeCount; i++)
        {
            tubes.Add(new TubeModle(targetLevelConfig.tubeHeight));
        }
        for (int i = 0; i < totalTubeCount; i++)
        {
            TubeData tubeDataFromJson = targetLevelConfig.tubes[i];

            foreach (string animalNameString in tubeDataFromJson.animals)
            {
                if (System.Enum.TryParse<AnimalType>(animalNameString, true, out AnimalType parsedAnimalType))
                {
                    tubes[i].animals.Add(parsedAnimalType);
                }

            }
        }

    }
    
    public MoveOperation MoveAnimal(int indexSource, int indexTarget)
    {
        MoveOperation moveOperation = null;
        if (indexSource < 0 || indexSource >= tubes.Count ||
            indexTarget < 0 || indexTarget >= tubes.Count ||
            indexSource == indexTarget)
        {
            return moveOperation;
        }
        TubeModle tubeModleSource = tubes[indexSource];
        TubeModle tubeModleTarget = tubes[indexTarget];

        if (!tubeModleSource.IsEmpty() && !tubeModleTarget.IsFull())
        {
            AnimalType? animalTypeFromSource = tubeModleSource.Peek();
            AnimalType? animalTypeOnTargetTop = tubeModleTarget.Peek();

            if (!animalTypeFromSource.HasValue)
            {
                return moveOperation;
            }
            bool canMoveToTargetBasedOnAnimal = CanTransferToTarget(tubeModleTarget, animalTypeFromSource, animalTypeOnTargetTop);

            if (canMoveToTargetBasedOnAnimal)
            {
                List<AnimalType> animalsMove = PopTopBlockOfSameType(tubeModleSource, tubeModleTarget, tubeModleTarget.NumberEmptyOfTube(), animalTypeFromSource);
                moveOperation = new MoveOperation(indexSource, indexTarget, animalsMove);
                return moveOperation;
            }
        }
        return moveOperation;
    }

    private bool CanTransferToTarget(TubeModle tubeModleTarget, AnimalType? animalTypeFromSource, AnimalType? animalTypeOnTargetTop)
    {
        if (tubeModleTarget.IsEmpty())
        {
            return true; 
        }
        if (animalTypeOnTargetTop.HasValue && animalTypeOnTargetTop.Value == animalTypeFromSource.Value)
        {
            return true;
        }
        return false; 
    }

    public List<AnimalType> PopTopBlockOfSameType(TubeModle tubeSource, TubeModle tubeTarget, int numberEmpty, AnimalType? typeToMatch)
    {
        List<AnimalType> animalMove = new List<AnimalType>();
        for (int i = 0; i < numberEmpty; i++)
        {   
            if (tubeSource.IsEmpty()) break;
            if (tubeTarget.IsFull()) break;

            AnimalType? currentSourceTop = tubeSource.Peek();

            if (currentSourceTop.HasValue &&  currentSourceTop == typeToMatch)
            {
                AnimalType? animalTypeTarget = tubeSource.Pop();
                if (animalTypeTarget != null)
                {
                    tubeTarget.AddAnimal(animalTypeTarget.Value);
                    animalMove.Add(animalTypeTarget.Value);
                }
            }
            else
            {
                break;
            }
        }
        return animalMove;
    }

 


    public void PrintTubesStateToConsole()
    {
        Debug.Log("---- Current Tubes State ----");
        for (int i = 0; i < tubes.Count; i++)
        {
            string animalsStr = string.Join(", ", tubes[i].animals.Select(a => a.ToString()));
            Debug.Log($"Tube {i} (Cap: {tubes[i].MaxCapacity}, Count: {tubes[i].animals.Count}): [{animalsStr}]");
        }
        Debug.Log("-----------------------------");
    }

    public HintMove FindNextHint()
    {
        HintMove hint = new HintMove();


        for (int i = 0; i < tubes.Count; i++) 
        {
            if (tubes[i].IsEmpty()) continue; 

            for (int j = 0; j < tubes.Count; j++) 
            {
                if (i == j) continue; 
                
               
                AnimalType? sourceTop = tubes[i].Peek();

                if (sourceTop.HasValue) 
                {
                    if (CanTransferToTargetLogic(tubes[j], sourceTop.Value, tubes[j].Peek()))
                    {
                        hint.SourceTubeIndex = i;
                        hint.DestinationTubeIndex = j;
                        hint.IsValid = true;
                        return hint; 
                    }
                }
            }
        }
        return hint; 
    }
    private bool CanTransferToTargetLogic(TubeModle tubeModleTarget, AnimalType actualAnimalTypeFromSource, AnimalType? animalTypeOnTargetTop)
    {
        if (tubeModleTarget.IsFull()) return false;
        if (tubeModleTarget.IsEmpty()) return true;
        if (animalTypeOnTargetTop.HasValue && animalTypeOnTargetTop.Value == actualAnimalTypeFromSource) return true;
        return false;
    }

    public void Undo(MoveOperation operation)
    {
        List<AnimalType> animalType = operation.MovedAnimals;
        int indexSource = operation.SourceTubeIndex;
        int indexDestination = operation.DestinationTubeIndex;
        foreach (AnimalType animal in animalType) 
        {
            tubes[indexSource].AddAnimal(animal);
            tubes[indexDestination].Pop();
        }
    }

    public bool IsLevelComplete()
    {
        if(tubes.Count == 0) return false;
        int countTubeSolve = 0;
        foreach (TubeModle tube in tubes)
        {
            if (tube.IsEmpty())
            {
                countTubeSolve++;
            }
            else
            {
                if (!tube.IsFull())
                {
                    return false; 
                }
                AnimalType? firstAnimalType = tube.Peek();
                if (tube.animals.All(animal => animal == firstAnimalType))
                {
                    countTubeSolve++;
                }
                else
                {
                    return false;
                }
            }
           
        }
        if(countTubeSolve == tubes.Count) return true;
        return false;
    }
}


