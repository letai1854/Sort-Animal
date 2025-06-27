using System.Collections.Generic;
using UnityEngine;

public class MoveOperation 
{
    public int SourceTubeIndex { get; }
    public int DestinationTubeIndex { get; }
    public List<AnimalType> MovedAnimals { get; }

    public MoveOperation( int sourceTubeIndex, int destinationTubeIndex, List<AnimalType> movedAnimals)
    {
        SourceTubeIndex = sourceTubeIndex;
        DestinationTubeIndex = destinationTubeIndex;
        MovedAnimals = new List<AnimalType>(movedAnimals); 
    }
    public int NumberOfAnimalsMoved => MovedAnimals?.Count ?? 0;

    public override string ToString()
    {
        return $" From: {SourceTubeIndex}, To: {DestinationTubeIndex}, Animals: [{string.Join(", ", MovedAnimals)}]";
    }
}
