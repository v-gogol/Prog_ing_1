using System;

namespace Abstract_Factory_Gogol
{
    internal class Program
    {
        abstract class SoundStream
        {
            public abstract string GetStreamDescription();
        }

        abstract class CaptionTrack
        {
            public abstract string GetCaptionDescription();
        }

        class RussianSound : SoundStream
        {
            public override string GetStreamDescription() => "Аудиопоток: Русский";
        }

        class RussianCaptions : CaptionTrack
        {
            public override string GetCaptionDescription() => "Текстовая дорожка: Русский";
        }

        class EnglishSound : SoundStream
        {
            public override string GetStreamDescription() => "Sound Stream: English";
        }

        class EnglishCaptions : CaptionTrack
        {
            public override string GetCaptionDescription() => "Caption Track: English";
        }

        class SpanishSound : SoundStream
        {
            public override string GetStreamDescription() => "Pista de Audio: Español";
        }

        class SpanishCaptions : CaptionTrack
        {
            public override string GetCaptionDescription() => "Subtítulos: Español";
        }

        interface ILocalizationFactory
        {
            SoundStream ProduceSoundStream();
            CaptionTrack ProduceCaptionTrack();
        }

        class RussianLocalizationFactory : ILocalizationFactory
        {
            public SoundStream ProduceSoundStream() => new RussianSound();
            public CaptionTrack ProduceCaptionTrack() => new RussianCaptions();
        }

 
        class EnglishLocalizationFactory : ILocalizationFactory
        {
            public SoundStream ProduceSoundStream() => new EnglishSound();
            public CaptionTrack ProduceCaptionTrack() => new EnglishCaptions();
        }

        class SpanishLocalizationFactory : ILocalizationFactory
        {
            public SoundStream ProduceSoundStream() => new SpanishSound();
            public CaptionTrack ProduceCaptionTrack() => new SpanishCaptions();
        }


        class LocalizedFilm
        {
            private SoundStream audioComponent;
            private CaptionTrack captionComponent;

            public LocalizedFilm(ILocalizationFactory factory)
            {
                audioComponent = factory.ProduceSoundStream();
                captionComponent = factory.ProduceCaptionTrack();
            }

            public void DisplayLocalizationInfo()
            {
                Console.WriteLine(audioComponent.GetStreamDescription());
                Console.WriteLine(captionComponent.GetCaptionDescription());
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Добро пожаловать в систему 'Кинодистрибуции'!");
            Console.WriteLine("Выберите язык контента: 1 - Русский, 2 - Английский, 3 - Испанский");

            string userSelection = Console.ReadLine();
            ILocalizationFactory localizationFactory;

            switch (userSelection)
            {
                case "1":
                    localizationFactory = new RussianLocalizationFactory();
                    break;
                case "2":
                    localizationFactory = new EnglishLocalizationFactory();
                    break;
                case "3":
                    localizationFactory = new SpanishLocalizationFactory();
                    break;
                default:
                    Console.WriteLine("Неверный выбор! Используется стандартный язык (русский).");
                    localizationFactory = new RussianLocalizationFactory();
                    return;
            }

            LocalizedFilm selectedFilm = new LocalizedFilm(localizationFactory);

            Console.WriteLine("\nВаш фильм подготовлен к просмотру!");
            Console.WriteLine("Параметры локализации:");
            selectedFilm.DisplayLocalizationInfo();

            Console.WriteLine("\nПриятного просмотра!");
        }
    }
}
