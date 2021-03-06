﻿using System;

namespace Ya.Taxi
{
    public class Field
    {
        public int Width
        {
            get { return _width; }
        }
        public int Height
        {
            get { return _height; }
        }
        public int XCoord
        {
            get { return _xCoord; }
        }
        public int YCoord
        {
            get { return _yCoord; }
        }
        public int[,] FieldArray
        {
            get { return _fieldArray; }
        }

        /// <summary>
        /// Constuctor with default parameters (W = 4, H = 4)
        /// </summary>
        public Field()
        {
            _width = 4;
            _height = 4;
            _xCoord = 0;
            _yCoord = 0;
            _fieldArray = GenerateFieldNumbers();
        }

        /// <summary>
        /// Constuctor with variying parameters
        /// </summary>
        /// <param name="width">Width of field</param>
        /// <param name="height">Height of field</param>
        public Field(int width, int height)
        {
            if (width < _minWidth || height < _minHeight || width > _maxWidth || height > _maxHeight)
            {
                throw new FieldSizeNotSupportedException($"Attempting to create a ({width},{height}) field. This size is not supported. Changing to valid size...");
            }
            _width = width;
            _height = height;
            _xCoord = 0;
            _yCoord = 0;
            _fieldArray = GenerateFieldNumbers();
        }

        /// <summary>
        /// Constuctor with variying parameters
        /// </summary>
        /// <param name="width">Width of field</param>
        /// <param name="height">Height of field</param>
        /// <param name="xCoord">Current X coord</param>
        /// <param name="yCoord">Current Y coord</param>
        public Field(int width, int height, int xCoord, int yCoord)
        {
            _width = width;
            _height = height;
            _xCoord = xCoord;
            _yCoord = yCoord;
            _fieldArray = GenerateFieldNumbers();
        }

        /// <summary>
        /// Do a movement to coordinates
        /// </summary>
        /// <param name="xCoord">Mentioned X coord</param>
        /// <param name="yCoord">Mentioned Y coord</param>
        public void Move(int xCoord, int yCoord)
        {
            _xCoord = xCoord;
            _yCoord = yCoord;
        }

        /// <summary>
        /// Reset the position to (0;0)
        /// </summary>
        public void ResetPosition()
        {
            _xCoord = 0;
            _yCoord = 0;
        }

        /// <summary>
        /// Print a field to console as an array
        /// </summary>
        public void ViewField()
        {
            Console.WriteLine("The field:");
            for (int i = 0; i < _fieldArray.GetLength(0); i++)
            {
                for (int j = 0; j < _fieldArray.GetLength(1); j++)
                {
                    if (i == _height - 1 && j == _width - 1)
                    {
                        Console.Write("{0}\t", "END".PadLeft(4));
                        break;
                    }

                    Console.Write("{0}\t", _fieldArray[i, j].ToString().PadLeft(4));
                }

                Console.WriteLine();
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Init _fieldArray by random numbers
        /// </summary>
        /// <returns>initialised _fieldArray</returns>
        private int[,] GenerateFieldNumbers()
        {
            int[,] numbers = new int[_width, _height];
            Random random = new Random();
            for (int i = 0; i < numbers.GetLength(0); i++)
            {
                for (int j = 0; j < numbers.GetLength(1); j++)
                {
                    numbers[i, j] = random.Next(_minRandomValue, _maxRandomValue + 1);
                }
            }

            return numbers;
        }

        private int[,] _fieldArray;

        private int _width,
            _height,
            _xCoord,
            _yCoord,
            _minRandomValue = -10,
            _maxRandomValue = 10,
            _minWidth = 2,
            _maxWidth = 32,
            _minHeight = 2,
            _maxHeight = 32;
    }


}