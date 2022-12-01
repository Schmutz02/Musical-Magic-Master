namespace MMM
{
    public class TimeConverter
    {
        public static int Time => (int) (UnityEngine.Time.time * 1000);
        public static int DeltaTime => (int) (UnityEngine.Time.deltaTime * 1000);
    }
}