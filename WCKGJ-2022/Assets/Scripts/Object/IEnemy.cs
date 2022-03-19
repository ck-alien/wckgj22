namespace EarthIsMine.Object
{
    public interface IEnemy
    {
        public int Life { get; set; }
        public bool IsDestroied { get; set; }

        public void Kill();
    }
}
