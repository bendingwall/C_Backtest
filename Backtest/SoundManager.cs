using System.Media;

namespace Backtest
{
    internal static class SoundManager
    {
        public static void PlayConnectionFinished()
        {
            SoundPlayer sound = new SoundPlayer(@"./Sounds/ConnectionFinished.wav");
            sound.Play();
        }
        public static void PlayConnectionLost()
        {
            SoundPlayer sound = new SoundPlayer(@"./Sounds/ConnectionLost.wav");
            sound.Play();
        }
        public static void PlayOrderCreated()
        {
            SoundPlayer sound = new SoundPlayer(@"./Sounds/OrderCreated.wav");
            sound.Play();
        }
        public static void PlayOrderFilled()
        {
            SoundPlayer sound = new SoundPlayer(@"./Sounds/OrderFilled.wav");
            sound.Play();
        }
        public static void PlayOrderRejected()
        {
            SoundPlayer sound = new SoundPlayer(@"./Sounds/OrderRejected.wav");
            sound.Play();
        }
        public static void PlayPositionsClosed()
        {
            SoundPlayer sound = new SoundPlayer(@"./Sounds/PositionsClosed.wav");
            sound.Play();
        }
        public static void PlayUpdate()
        {
            SoundPlayer sound = new SoundPlayer(@"./Sounds/Update.wav");
            sound.Play();
        }
    }
}
