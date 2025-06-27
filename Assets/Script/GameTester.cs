// GameTester.cs (gắn vào một GameObject trong Scene)
using UnityEngine;

public class GameTester : MonoBehaviour
{
    void Start()
    {
        TestGameGeneration();
    }

    void TestGameGeneration()
    {
        //Debug.Log("Testing GameModle Generation...");

        //// Test case 1
        //Debug.Log("--- Test Case 1: 5 tubes, height 4, 2 empty ---");
        //GameModle game1 = new GameModle(4, 4, 1);
        //game1.PrintTubesStateToConsole();


        //// Test case 2
        //Debug.Log("--- Test Case 2: 7 tubes, height 3, 2 empty ---");
        //GameModle game2 = new GameModle(7, 3, 2);
        //game2.PrintTubesStateToConsole();

        //// Test case 3: Sử dụng hết các loại animal trong enum
        //int maxEnumTypes = System.Enum.GetValues(typeof(AnimalType)).Length;
        //Debug.Log($"--- Test Case 3: Using all {maxEnumTypes} animal types ---");
        //GameModle game3 = new GameModle(maxEnumTypes + 2, 8, 2); // +2 ống trống
        //game3.PrintTubesStateToConsole();
    }
}