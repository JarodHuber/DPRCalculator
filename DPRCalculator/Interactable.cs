using Raylib_cs;

namespace DPRCalculator
{
    public interface Interactable
    {
        public bool AttemptInteract();

        public void Update();
        public void Draw();
    }
}
