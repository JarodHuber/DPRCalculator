using Raylib_cs;

namespace DPRCalculator
{
    public interface IInteractable
    {
        public bool AttemptInteract();

        public void Update();
        public void Draw();
    }
}
