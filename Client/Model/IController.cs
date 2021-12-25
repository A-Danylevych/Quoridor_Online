namespace Model
{
    public interface IController
    {
        void SetCell(int top, int left);
        void SetWall(int top, int left, bool isVertical);
    }
}