using Components;
using Utils;

namespace Entities
{
    public class Gem : PoolingObject<Gem>
    {
        public int X;
        public int Y;

        public void Fall()
        {
            var falling = gameObject.AddComponent<FallingComponent>();
            falling.OnComplete = Release;
        }

        public void Collect()
        {
            var flying = gameObject.AddComponent<FlyingComponent>();
            flying.OnComplete = Release;
        }

        public override void Reset()
        {
        }
    }
}