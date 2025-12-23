using System;
using System.Collections.Generic;

namespace Singleton_Gogol
{
    internal class Program
    {
        public class Skyscraper
        {
            private static Skyscraper _mainBuilding;

            private VerticalTransport _transportSystem;
            private List<BuildingLevel> _buildingLevels;

            public static Skyscraper GetInstance()
            {
                if (_mainBuilding == null)
                {
                    _mainBuilding = new Skyscraper();
                }
                return _mainBuilding;
            }

            private Skyscraper()
            {
                Console.WriteLine("Инициализация строения");
                _transportSystem = VerticalTransport.GetTransport();
                _buildingLevels = new List<BuildingLevel>();
                InitializeLevels();
            }

            private void InitializeLevels()
            {
                for (int level = 1; level <= 10; level++)
                {
                    _buildingLevels.Add(new BuildingLevel(level));
                }
            }

            public void RequestLift(int targetLevel)
            {
                if (targetLevel < 1 || targetLevel > _buildingLevels.Count)
                {
                    Console.WriteLine($"Уровень {targetLevel} отсутствует в строении");
                    return;
                }

                _transportSystem.MoveTo(targetLevel, _buildingLevels[targetLevel - 1]);
            }
        }

        public struct BuildingLevel
        {
            public int LevelNumber { get; }
            public string OfficeA { get; }
            public string OfficeB { get; }

            public BuildingLevel(int number)
            {
                LevelNumber = number;
                OfficeA = $"Офис {number}А";
                OfficeB = $"Офис {number}Б";
            }

            public void DisplayOffices()
            {
                Console.WriteLine($"На уровне {LevelNumber}: {OfficeA}, {OfficeB}");
            }
        }

        public class VerticalTransport
        {
            private static VerticalTransport _transportSystem;
            private int _currentPosition;

            public static VerticalTransport GetTransport()
            {
                if (_transportSystem == null)
                {
                    _transportSystem = new VerticalTransport();
                }
                return _transportSystem;
            }

            private VerticalTransport()
            {
                _currentPosition = 1;
                Console.WriteLine("Транспортная система установлена на уровне 1");
            }

            public void MoveTo(int destination, BuildingLevel level)
            {
                if (destination == _currentPosition)
                {
                    Console.WriteLine($"Лифт уже на уровне {_currentPosition}");
                    level.DisplayOffices();
                    return;
                }

                Console.WriteLine($"Лифт перемещается с {_currentPosition} на {destination} уровень");

                if (destination > _currentPosition)
                {
                    Console.WriteLine("Подъем...");
                }
                else
                {
                    Console.WriteLine("Спуск...");
                }

                _currentPosition = destination;
                Console.WriteLine($"Лифт прибыл на {_currentPosition} уровень");
                level.DisplayOffices();
                Console.WriteLine();
            }
        }

        static void Main(string[] args)
        {
            var tower = Skyscraper.GetInstance();

            tower.RequestLift(5);
            tower.RequestLift(2);
            tower.RequestLift(15);
        }
    }
}