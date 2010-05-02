using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace MLib.DataManipulation
{
    public class DataGridViewData
    {
        #region GetCellAverage
        /// <summary>
        /// Calculates the average number of a given cell in DataGridView
        /// </summary>
        /// <param name="DGV">DataGridView that will be calculated</param>
        /// <param name="Cell_Number">Cell number of the values</param>
        /// <returns>The average value in the given cell</returns>
        public static double GetCellAverage(DataGridView DGV, int Cell_Number)
        {
            return GetCellAverage(DGV, Cell_Number, 0, -1);
        }

        /// <summary>
        /// Calculates the average number of a given cell in DataGridView
        /// </summary>
        /// <param name="DGV">DataGridView that will be calculated</param>
        /// <param name="Cell_Number">Cell number of the values</param>
        /// <param name="Start">Starting index for calculations</param>
        /// <param name="End">Ending index for calculations. -1 will act as DataGridView.Rows.Count</param>
        /// <returns>The average value in the given cell</returns>
        public static double GetCellAverage(DataGridView DGV, int Cell_Number, int Start, int End)
        {
            if (End == -1)
                End = DGV.Rows.Count;

            if(End > DGV.Rows.Count)
                End = DGV.Rows.Count;

            double Num = 0;
            double Val = 0;
            for (int i = Start; i < End; i++)
            {
                try
                {
                    if (DGV.Rows[i].Cells[Cell_Number].Value != null)
                    {
                        Val += Convert.ToDouble(DGV.Rows[i].Cells[Cell_Number].Value);
                        Num++;
                    }
                }
                catch { }
            }

            return Val / Num;
        }
        #endregion

        #region GetCellMinimum
        /// <summary>
        /// Calculates the minimum number of a given cell in DataGridView
        /// </summary>
        /// <param name="DGV">DataGridView that will be calculated</param>
        /// <param name="Cell_Number">Cell number of the values</param>
        /// <returns>The minimum value in the given cell</returns>
        public static double GetCellMinimum(DataGridView DGV, int Cell_Number)
        {
            return GetCellMinimum(DGV, Cell_Number, 0, -1);
        }

        /// <summary>
        /// Calculates the minimum value of a given cell in DataGridView
        /// </summary>
        /// <param name="DGV">DataGridView that will be calculated</param>
        /// <param name="Cell_Number">Cell number of the values</param>
        /// <param name="Start">Starting index for calculations</param>
        /// <param name="End">Ending index for calculations. -1 will act as DataGridView.Rows.Count</param>
        /// <returns>The minimum value in the given cell</returns>
        public static double GetCellMinimum(DataGridView DGV, int Cell_Number, int Start, int End)
        {
            if (End == -1)
                End = DGV.Rows.Count;

            if (End > DGV.Rows.Count)
                End = DGV.Rows.Count;

            double Min = double.MaxValue;
            for (int i = Start; i < End; i++)
            {
                try
                {
                    if (DGV.Rows[i].Cells[Cell_Number].Value != null)
                    {
                        double N = Convert.ToDouble(DGV.Rows[i].Cells[Cell_Number].Value);

                        if (N <= Min)
                            Min = N;
                    }
                }
                catch { }
            }

            return Min;
        }
        #endregion

        #region GetCellMaximum
        /// <summary>
        /// Calculates the maximum number of a given cell in DataGridView
        /// </summary>
        /// <param name="DGV">DataGridView that will be calculated</param>
        /// <param name="Cell_Number">Cell number of the values</param>
        /// <returns>The maximum value in the given cell</returns>
        public static double GetCellMaximum(DataGridView DGV, int Cell_Number)
        {
            return GetCellMaximum(DGV, Cell_Number, 0, -1);
        }

        /// <summary>
        /// Calculates the maximum value of a given cell in DataGridView
        /// </summary>
        /// <param name="DGV">DataGridView that will be calculated</param>
        /// <param name="Cell_Number">Cell number of the values</param>
        /// <param name="Start">Starting index for calculations</param>
        /// <param name="End">Ending index for calculations. -1 will act as DataGridView.Rows.Count</param>
        /// <returns>The maximum value in the given cell</returns>
        public static double GetCellMaximum(DataGridView DGV, int Cell_Number, int Start, int End)
        {
            if (End == -1)
                End = DGV.Rows.Count;

            if (End > DGV.Rows.Count)
                End = DGV.Rows.Count;

            double Max = -double.MaxValue;
            for (int i = Start; i < End; i++)
            {
                try
                {
                    if (DGV.Rows[i].Cells[Cell_Number].Value != null)
                    {
                        double N = Convert.ToDouble(DGV.Rows[i].Cells[Cell_Number].Value);

                        if (N >= Max)
                            Max = N;
                    }
                }
                catch { }
            }

            return Max;
        }
        #endregion

    }
}
