using System;

namespace BojkoSoft.Transformations.TransformationModels
{
    /// <summary>
    /// Class for dealing with matrices
    /// </summary>
    internal class Matrix
    {
        private readonly double[,] data;

        /// <summary>
        /// Creates a new Matrix with specified size
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="cols"></param>
        public Matrix(int rows, int cols)
        {
            this.data = new double[rows, cols];
        }

        /// <summary>
        /// Get a matrix element
        /// </summary>
        /// <param name="x">row number</param>
        /// <param name="y">column number</param>
        /// <returns></returns>
        public double this[int x, int y]
        {
            get { return data[x, y]; }
            set { data[x, y] = value; }
        }

        /// <summary>
        /// Inverts this matrix
        /// See: http://csharphelper.com/blog/2016/05/find-matrix-inverse-c/
        /// </summary>
        /// <returns></returns>
        public Matrix Invert()
        {
            const double tiny = 0.00001;

            // Build the augmented matrix.
            int num_rows = this.data.GetUpperBound(0) + 1;
            Matrix augmented = new Matrix(num_rows, 2 * num_rows);
            for (int row = 0; row < num_rows; row++)
            {
                for (int col = 0; col < num_rows; col++)
                    augmented[row, col] = this.data[row, col];
                augmented[row, row + num_rows] = 1;
            }

            // num_cols is the number of the augmented matrix.
            int num_cols = 2 * num_rows;

            // Solve.
            for (int row = 0; row < num_rows; row++)
            {
                // Zero out all entries in column r after this row.
                // See if this row has a non-zero entry in column r.
                if (Math.Abs(augmented[row, row]) < tiny)
                {
                    // Too close to zero. Try to swap with a later row.
                    for (int r2 = row + 1; r2 < num_rows; r2++)
                    {
                        if (Math.Abs(augmented[r2, row]) > tiny)
                        {
                            // This row will work. Swap them.
                            for (int c = 0; c < num_cols; c++)
                            {
                                double tmp = augmented[row, c];
                                augmented[row, c] = augmented[r2, c];
                                augmented[r2, c] = tmp;
                            }
                            break;
                        }
                    }
                }

                // If this row has a non-zero entry in column r, use it.
                if (Math.Abs(augmented[row, row]) > tiny)
                {
                    // Divide the row by augmented[row, row] to make this entry 1.
                    for (int col = 0; col < num_cols; col++)
                        if (col != row)
                            augmented[row, col] /= augmented[row, row];
                    augmented[row, row] = 1;

                    // Subtract this row from the other rows.
                    for (int row2 = 0; row2 < num_rows; row2++)
                    {
                        if (row2 != row)
                        {
                            double factor = augmented[row2, row] / augmented[row, row];
                            for (int col = 0; col < num_cols; col++)
                                augmented[row2, col] -= factor * augmented[row, col];
                        }
                    }
                }
            }

            // See if we have a solution.
            if (augmented[num_rows - 1, num_rows - 1] == 0) return null;

            // Extract the inverse array.
            Matrix inverse = new Matrix(num_rows, num_rows);
            for (int row = 0; row < num_rows; row++)
            {
                for (int col = 0; col < num_rows; col++)
                {
                    inverse[row, col] = augmented[row, col + num_rows];
                }
            }

            return inverse;
        }
    }
}
