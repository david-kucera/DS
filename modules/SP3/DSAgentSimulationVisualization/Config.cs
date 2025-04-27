using System.Drawing;

namespace DSAgentSimulationVisualization
{
    public static class Config
    {
        #region Paths
        public static string BackgroundPath { get; set; } = "../../resources/podlaha.jpg";
        #endregion // Paths

        #region Properties
        public static Image Background { get; set; } = Image.FromFile(BackgroundPath);
        #endregion // Properties
    }
}
