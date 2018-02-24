using System;
using System.Collections.Generic;
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
            get { return _succedeedMovements; }
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
                Console.WriteLine($"A FieldSizeNotSupportedException is occured in {e.TargetSite}\n{e.Message}\nPlease, set valid size\nThe field will be default-sized on this time\n");
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
            _movements = GenerateRoutes();
            _succedeedMovements = new Dictionary<int, string>();
            foreach (string movement in _movements)
            {
                _field.ResetPosition();
                if (TryRoute(movement) && !_succedeedMovements.ContainsKey(RouteNumbersSum(movement)))
                {
                    _succedeedMovements.Add(RouteNumbersSum(movement), movement);
                    Console.WriteLine($"Success: route found [{movement}] with weight [{RouteNumbersSum(movement)}]");
                }
            }
        }

        /// <summary>
        /// Try a route by string representation where 0 is "go down" and 1 is "go right"
        /// NOTE: Considering as slowing function
        /// </summary>
        /// <param name="route"></param>
        /// <returns></returns>
        private bool TryRoute(string route)
        {
            foreach (char c in route)
            {
                switch (c)
                {
                    case '0':
                        if (_field.IsValidMovement(_field.XCoord, _field.YCoord + 1))
                        {
                            _field.Move(_field.XCoord, _field.YCoord + 1);
                        }
                        else
                        {
                            return false;
                        }
                        break;
                    case '1':
                        if (_field.IsValidMovement(_field.XCoord + 1, _field.YCoord))
                        {
                            _field.Move(_field.XCoord + 1, _field.YCoord);
                        }
                        else
                        {
                            return false;
                        }
                        break;
                }

                if (_field.XCoord == _field.Width - 1 && _field.YCoord == _field.Height - 1)
                {
                    //Console.WriteLine($"Route:{route}");
                    return true;
                }
            }

            return (_field.XCoord == _field.Width - 1 && _field.YCoord == _field.Height - 1);
        }

        /// <summary>
        /// Calculate a sum of the route (a significant part without finish point)
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
                        _field.Move(_field.XCoord, _field.YCoord + 1);
                        break;
                    case '1':
                        sum += _field.FieldArray[_field.XCoord, _field.YCoord];
                        _field.Move(_field.XCoord + 1, _field.YCoord);
                        break;
                }

                if (_field.XCoord == _field.Width - 1 && _field.YCoord == _field.Height - 1) return sum;
            }
            return sum;
        }

        /// <summary>
        /// Generate all the routes
        /// NOTE: considered as slowing function
        /// </summary>
        /// <returns></returns>
        private LinkedList<string> GenerateRoutes()
        {
            LinkedList<string> routes = new LinkedList<string>();
            int bitCapacityNeeded = _field.FieldArray.Length / 2;
            for (int i = 0; i < Math.Pow(2, bitCapacityNeeded); i++)
                routes.AddLast(FormatRoute(Convert.ToString(i, 2), bitCapacityNeeded));
            return routes;
        }

        /// <summary>
        /// Adds insignificant zeros to the required length
        /// </summary>
        /// <param name="route">source route</param>
        /// <param name="bitCapacity">required length</param>
        /// <returns></returns>
        private static string FormatRoute(string route, int bitCapacity)
        {
            if (!(route.Length < bitCapacity))
            {
                return route;
            }
            StringBuilder sb = new StringBuilder();
            sb.Append('0', bitCapacity - route.Length).Append(route);
            return sb.ToString();
        }

        private Field _field;
        private LinkedList<string> _movements;
        private Dictionary<int, string> _succedeedMovements;
    }
}