using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class TubeModle
{
    public List<AnimalType> animals { get; protected set; }
    public int MaxCapacity { get; private set; }
    
    public TubeModle(int maxCapacity)
    {
        MaxCapacity = maxCapacity;
        animals = new List<AnimalType>();
    }
    public bool IsFull() => animals.Count >= MaxCapacity;
    public bool IsEmpty() => animals.Count == 0;

    public bool AddAnimal(AnimalType animal)
    {
        if (IsFull())
        {
            return false;
        }
        animals.Add(animal);
        return true;
    }
    public AnimalType? Pop() 
    {
        if (!IsEmpty())
        {
            AnimalType lastAnimal = animals[^1];
            animals.RemoveAt(animals.Count-1);
            return lastAnimal;
        }
        return null;
    }
    public AnimalType? Peek()
    {
        return IsEmpty() ? null : animals[^1];
    }
    public int NumberEmptyOfTube()
    {
        return MaxCapacity - animals.Count;
    }
}
