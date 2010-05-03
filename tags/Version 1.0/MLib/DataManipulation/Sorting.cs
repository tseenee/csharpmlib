using System;
using System.Collections.Generic;
using System.Text;

namespace MLib.DataManipulation
{
    public static class Sorting
    {
        #region QuickSort
        /// <summary>
        /// Uses QuickSort method to sort an array of Integers
        /// </summary>
        /// <param name="Array">Array to be sorted</param>
        public static void ArrayQuickSort(int[] Array)
        {
            int x = Array.Length;
            int i, j, increment, temp;

            increment = 3;

            while (increment > 0)
            {
                for (i = 0; i < x; i++)
                {
                    j = i;
                    temp = Array[i];

                    while ((j >= increment) && (Array[j - increment] > temp))
                    {
                        Array[j] = Array[j - increment];
                        j = j - increment;
                    }

                    Array[j] = temp;
                }

                if (increment / 2 != 0)
                {
                    increment = increment / 2;
                }
                else if (increment == 1)
                {
                    increment = 0;
                }
                else
                {
                    increment = 1;
                }
            }
        }
        #endregion

        #region Randomize
        /// <summary>
        /// Randomizes the values in an Array
        /// </summary>
        /// <param name="Array">Array to be randomized</param>
        public static void Randomize(int[] Array)
        {
            int[] temp = Array;
            Random rand = new Random();
            for (int i = 0; i < Array.Length; i++)
                Array[i] = temp[rand.Next(0, Array.Length)];
        }
        #endregion

        #region Lowest, Highest, Average

        /// <summary>
        /// Finds the lowest value in the given array
        /// </summary>
        /// <param name="Array">Array to be searched</param>
        /// <returns></returns>
        public static int GetLowestValue(int[] Array)
        {
            int lowest = Array[0];
            foreach (int stev in Array)
                if (stev < lowest)
                    lowest = stev;

            return lowest;
        }

        /// <summary>
        /// Finds the highest value in the given array
        /// </summary>
        /// <param name="Array">Array to be searched</param>
        /// <returns></returns>
        public static int GetHighestValue(int[] Array)
        {
            int highest = Array[0];
            foreach (int stev in Array)
                if (stev > highest)
                    highest = stev;

            return highest;
        }

        /// <summary>
        /// Calculates the average value of an integer array
        /// </summary>
        /// <param name="Array">Array to be calculated</param>
        /// <returns></returns>
        public static int GetAverage(int[] Array)
        {
            int together = 0;
            foreach (int stev in Array)
                together += stev;

            return (together / Array.Length);
        }

        #endregion
    }
}
