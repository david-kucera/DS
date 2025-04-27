using System.Drawing;

namespace DSAgentSimulationWoodwork.Entities
{
    public static class Config
    {
        #region Paths
        public static string IMG_PATH_SKLAD { get; set; } = "../../resources/sklad.png";
        #endregion // Paths

        #region Constants
        public static int SKLAD_POS_X { get; set; } = 0;
        public static int SKLAD_POS_Y { get; set; } = 0;
        public static int SKLAD_WIDTH { get; set; } = 300;
        public static int SKLAD_HEIGHT { get; set; } = 300;
        public static Random Random { get; set; } = new Random();
        #endregion // Constants

        #region Properties
        public static Image Sklad { get; set; } = Image.FromFile(IMG_PATH_SKLAD);
        #endregion // Properties

        #region Public functions
        public static int GetRandomSkladPosX()
        {
            return SKLAD_POS_X + Random.Next(SKLAD_WIDTH)/2;
        }

        public static int GetRandomSkladPosY()
        {
            return SKLAD_POS_Y + Random.Next(SKLAD_HEIGHT)/2;
        }
        #endregion // Public functions
    }
}
