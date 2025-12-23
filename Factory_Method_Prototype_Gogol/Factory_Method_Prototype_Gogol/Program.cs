using System;
using System.Collections.Generic;

namespace Factory_Method_Prototype_Gogol
{
    internal class Program
    {
        public abstract class FigureForm : ICloneable
        {
            public abstract string Title { get; }
            public abstract int TileCount { get; }
            public abstract object Clone();

            public void Display()
            {
                Console.WriteLine($"{Title} (Элементов: {TileCount})");
            }
        }

        public class StraightForm : FigureForm
        {
            public override string Title => "Прямая";
            public override int TileCount => 4;

            public override object Clone()
            {
                return new StraightForm();
            }
        }

        public class BoxForm : FigureForm
        {
            public override string Title => "Квадрат";
            public override int TileCount => 4;

            public override object Clone()
            {
                return new BoxForm();
            }
        }

        public class TeeForm : FigureForm
        {
            public override string Title => "T-форма";
            public override int TileCount => 4;

            public override object Clone()
            {
                return new TeeForm();
            }
        }

        public class LongStraightForm : FigureForm
        {
            public override string Title => "Длинная прямая";
            public override int TileCount => 6;

            public override object Clone()
            {
                return new LongStraightForm();
            }
        }

        public class PlusForm : FigureForm
        {
            public override string Title => "Плюс";
            public override int TileCount => 5;

            public override object Clone()
            {
                return new PlusForm();
            }
        }

        public class FormGenerator
        {
            private readonly List<FigureForm> _baseForms;
            private readonly Random _randomizer;

            public FormGenerator()
            {
                _randomizer = new Random();
                _baseForms = new List<FigureForm>
            {
                new StraightForm(),
                new BoxForm(),
                new TeeForm(),
                new LongStraightForm(),
                new PlusForm()
            };
            }

            public FigureForm GenerateForm()
            {
                int formIndex = _randomizer.Next(_baseForms.Count);
                return (FigureForm)_baseForms[formIndex].Clone();
            }
        }

        static void Main(string[] args)
        {
            FormGenerator generator = new FormGenerator();

            for (int counter = 0; counter < 5; counter++)
            {
                FigureForm originalForm = generator.GenerateForm();
                FigureForm duplicatedForm = (FigureForm)originalForm.Clone();

                Console.WriteLine("Исходная форма:");
                originalForm.Display();

                Console.WriteLine("Дубликат:");
                duplicatedForm.Display();

                Console.WriteLine(new string('-', 35));
            }
        }
    }
}
