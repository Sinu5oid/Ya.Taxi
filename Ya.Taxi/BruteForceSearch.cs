using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ya.Taxi
{
    public class BruteForceSearch
    {
        /// <summary>
        /// Return a dictionary with succeeded movements (distinct by weights)
        /// </summary>
        public Dictionary<int, string> SuccededMovementDictionary
        {
            get { return _successfulMovements; }
        }

        /// <summary>
        /// Constructor by default (5x5 field)
        /// </summary>
        public BruteForceSearch()
        {
            _field = new Field();
            DoBruteForce();
        }

        /// <summary>
        /// Constructor with variying parameters
        /// </summary>
        /// <param name="width">Mentioned width</param>
        /// <param name="height">Mentioned height</param>
        public BruteForceSearch(int width, int height)
        {
            try
            {
                _field = new Field(width, height);
            }
            catch (FieldSizeNotSupportedException e)
            {
                Console.WriteLine($"{e.Message}\n");
                _field = new Field();
            }

            DoBruteForce();
        }

        /// <summary>
        /// Main class action
        /// </summary>
        private void DoBruteForce()
        {
            _field.ViewField();
            Console.WriteLine("Building routes...");
            _successfulMovements = new Dictionary<int, string>();
            int bitCapacityNeeded = (_field.Height - 1) + (_field.Width - 1);
            Int64 rangeFloor =
                    Convert.ToInt64(
                        new StringBuilder().Append('0', (_field.Height + _field.Width - 2) / 2)
                            .Append('1', (_field.Height + _field.Width - 2) / 2).ToString(), 2),
                rangeCeiling = (Int64)Math.Pow(2, bitCapacityNeeded);
            for (Int64 i = rangeFloor; i < rangeCeiling; i++)
            {
                string movement = FormatRoute(Convert.ToString(i, 2), bitCapacityNeeded);
                _field.ResetPosition();
                int sum;
                if (IsValidRoute(movement) && !_successfulMovements.ContainsKey(sum = RouteNumbersSum(movement)))
                {
                    _successfulMovements.Add(sum, movement);
                    Console.WriteLine($"Route found [{movement}]" + " with weight [{0}]", sum.ToString().PadLeft(4));
                }
            }
            Console.WriteLine("All routes built.");
        }

        /// <summary>
        /// Try a route by string representation where 0 is "go down" and 1 is "go right"
        /// NOTE: Considering as slowing function
        /// </summary>
        /// <param name="route"></param>
        /// <returns></returns>
        private bool IsValidRoute(string route)
        {
            int enabledBitCount = route.Count(p => p == '1');
            return (enabledBitCount == _field.Height - 1) && (route.Length - enabledBitCount == _field.Width - 1);
        }

        /// <summary>
        /// Calculate a sum of the route (without END point)
        /// </summary>
        /// <param name="route">route represented by 0 and 1</param>
        /// <returns></returns>
        private int RouteNumbersSum(string route)
        {
            _field.ResetPosition();
            int sum = 0;
            foreach (char c in route)
            {
                switch (c)
                {
                    case '0':

                        sum += _field.FieldArray[_field.XCoord, _field.YCoord];
                        _field.Move(_field.XCoord + 0, _field.YCoord);
                        break;
                    case '1':
                        sum += _field.FieldArray[_field.XCoord, _field.YCoord];
                        _field.Move(_field.XCoord, _field.YCoord + 1);
                        break;
                }

                if (_field.XCoord == _field.Width - 1 && _field.YCoord == _field.Height - 1) return sum;
            }
            return sum;
        }

        /// <summary>
        /// Adds insignificant zeros to the required length
        /// </summary>
        /// <param name="route">source route</param>
        /// <param name="bitCapacity">required length</param>
        /// <returns></returns>
        private static string FormatRoute(string route, int bitCapacity)
        {
            return (route.Length < bitCapacity) ? new StringBuilder().Append('0', bitCapacity - route.Length).Append(route).ToString() : route;
        }

        private Field _field;
        private Dictionary<int, string> _successfulMovements;
    }
}