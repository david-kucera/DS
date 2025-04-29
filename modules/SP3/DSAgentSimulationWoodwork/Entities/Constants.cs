using System.Drawing;

namespace DSAgentSimulationWoodwork.Entities
{
    public static class Constants
    {
        private const int ANIM_QUEUE_START_X = 0;
        private const int ANIM_QUEUE_START_Y = 500;
        public static PointF ANIM_QUEUE_START = new(ANIM_QUEUE_START_X, ANIM_QUEUE_START_Y);
        private const int ANIM_QUEUE_END_X = 500;
        private const int ANIM_QUEUE_END_Y = 500;
        public static PointF ANIM_QUEUE_END = new(ANIM_QUEUE_END_X, ANIM_QUEUE_END_Y);
        public const int ANIM_QUEUE_SPEED = 50;
    }
}
