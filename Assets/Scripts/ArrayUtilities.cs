using UnityEngine;

public static class ArrayUtilities
{
    ////we use this extension method to shuffle array elements
    public static void Shuffle<T>(this T[] array)
    {
        T varExchange;
        int firstRandElement = 0;
        int secondRandElement = 0;

        for (int i = 0; i < array.Length; i++)
        {
            firstRandElement = Random.Range(0, array.Length);

            do
            {
                secondRandElement = Random.Range(0, array.Length);
            }
            while (firstRandElement == secondRandElement);

            //array elements swap
            varExchange = array[firstRandElement];

            array[firstRandElement] = array[secondRandElement];
            array[secondRandElement] = varExchange;
        }
    }
}
