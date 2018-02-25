using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Ya.Taxi
{
    public class BruteForceSearch
    {
        /// <summary>
        /// Return a dictionary with succeeded movements (distinct by weights)
        /// </summary>
        public Dictionary<int, string> SuccededMovementDictionary => _successfulMovements;

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
            int bitCapacityNeeded;
            long rangeFloor, rangeCeiling;
            lock (_field)
            {
                _field.ViewField();
                Console.WriteLine("Building routes...");
                _successfulMovements = new Dictionary<int, string>();
                bitCapacityNeeded = (_field.Height - 1) + (_field.Width - 1);
                rangeFloor =
                    Convert.ToInt64(
                        new StringBuilder().Append('0', (_field.Height + _field.Width - 2) / 2)
                            .Append('1', (_field.Height + _field.Width - 2) / 2).ToString(), 2);
                rangeCeiling = Convert.ToInt64(
                    new StringBuilder().Append('1', (_field.Height + _field.Width - 2) / 2)
                        .Append('0', (_field.Height + _field.Width - 2) / 2).ToString(), 2);
            }

            Thread[] threads = new Thread[_threadsCount];
            Parameter[] parameters = new Parameter[_threadsCount];
            for (int i = 0; i < _threadsCount; i++)
            {
                threads[i] = new Thread(PerformUniqueTask);
                parameters[i] = new Parameter(_threadsCount, i, rangeFloor, rangeCeiling, bitCapacityNeeded);
                threads[i].Start(parameters[i]);
            }
            while (threads.Any(t => t.IsAlive))
                Thread.Yield();
            Console.WriteLine("All routes built.");
        }

        /// <summary>
        /// Inner class to use in DoInThread
        /// </summary>
        class Parameter
        {
            public Parameter(int threadsCount, int shift, long rangeFloor, long rangeCeiling, int bitCapacityNeeded)
            {
                ThreadsCount = threadsCount;
                Shift = shift;
                RangeFloor = rangeFloor;
                RangeCeiling = rangeCeiling;
                BitCapacityNeeded = bitCapacityNeeded;
            }

            public int ThreadsCount { get; }
            public int Shift { get; }
            public int BitCapacityNeeded { get; }
            public long RangeCeiling { get; }
            public long RangeFloor { get; }
        }

        /// <summary>
        /// Perform one unique task within field
        /// </summary>
        /// <param name="o">Parameter object</param>
        private void PerformUniqueTask(object o)
        {
            Parameter p = (Parameter)o;

            for (long i = p.Shift + p.RangeFloor; i <= p.RangeCeiling; i += p.ThreadsCount)
            {
                string movement = FormatRoute(Convert.ToString(i, 2), p.BitCapacityNeeded);
                if (IsValidRoute(movement))
                {
                    lock (_successfulMovements)
                    {
                        int sum;
                        if (!_successfulMovements.ContainsKey(sum = RouteNumbersSum(movement)))
                        {
                            _successfulMovements.Add(sum, movement);
                            Console.WriteLine($"Route found [{movement}]" + " with weight [{0}]",
                                sum.ToString().PadLeft(4));
                        }
                    }
                }
            }
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
            lock (_field)
            {
                _field.ResetPosition();
                int sum = 0;
                foreach (char c in route)
                {
                    switch (c)
                    {
                        case '0':

                            sum += _field.FieldArray[_field.XCoord, _field.YCoord];
                            _field.Move(_field.XCoord + 1, _field.YCoord);
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
        private readonly int _threadsCount = Environment.ProcessorCount;
    }
}