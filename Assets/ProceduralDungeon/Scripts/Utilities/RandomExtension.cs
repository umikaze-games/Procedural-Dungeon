using System.Collections.Generic;
using Random = System.Random;


public static class NumberSelection
{
    public static List<int> RandomNumbersWithMinDistance(int n, int min, int max, int minDistance)
    {
        Random random = new Random();

        List<int> selectedNumbers = new List<int>();
        List<int> availableNumbers = new List<int>();
        for (var i = min; i < max; i++)
        {
            availableNumbers.Add(i);
        }

        for (var i = 0; i < n; i++)
        {
            if (availableNumbers.Count == 0)
            {
                break;
            }
            var randomIndex = random.Next(0, availableNumbers.Count);
            var selectedNumber = availableNumbers[randomIndex];
            selectedNumbers.Add(selectedNumber);

            int minBound = selectedNumber - minDistance;
            int maxBound = selectedNumber + minDistance;

            availableNumbers.RemoveAll(num => num >= minBound && num <= maxBound);
        }

        return selectedNumbers;
    }
}

