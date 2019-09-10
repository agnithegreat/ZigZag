using Components;
using Utils;

namespace Entities
{
    public class Tile : PoolingObject<Tile>
    {
        public int X;
        public int Y;

        public void Fall()
        {
            var falling = gameObject.AddComponent<FallingComponent>();
            falling.OnComplete = Release;
        }

        public override void Reset()
        {
        }
    }
}