using System.Drawing;

namespace DSAgentSimulationWoodwork.Entities
{
    public static class Sklad
    {
        #region Paths
        private const string IMG_PATH_SKLAD = "../../resources/sklad.png";
        #endregion // Paths

        #region Constants
        public static int SKLAD_POS_X { get; set; } = 0;
        public static int SKLAD_POS_Y { get; set; } = 0;
        public static int SKLAD_WIDTH { get; set; } = 400;
        public static int SKLAD_HEIGHT { get; set; } = 400;
        public static Random Random { get; set; } = new Random();
        #endregion // Constants

        #region Properties
        public static Image Image { get; set; } = Image.FromFile(IMG_PATH_SKLAD);
        #endregion // Properties

        #region Public functions
        public static int GetRandomSkladPosX()
        {
            return SKLAD_POS_X + Random.Next(SKLAD_WIDTH - (SKLAD_WIDTH/4));
        }

        public static int GetRandomSkladPosY()
        {
            return SKLAD_POS_Y + Random.Next(SKLAD_HEIGHT - (SKLAD_HEIGHT/4));
        }
        #endregion // Public functions
    }
}
