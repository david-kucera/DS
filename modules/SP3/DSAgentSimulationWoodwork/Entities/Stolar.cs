using System.IO;
using DSAgentSimulationLib.Statistics;
using OSPAnimator;

namespace DSAgentSimulationWoodwork.Entities;

public class Stolar
{
    private static int _poradie = 1;

    #region Constants
    private const string IMG_PATH_A = "../../resources/stolarA.png";
    private const string IMG_PATH_B = "../../resources/stolarB.png";
    private const string IMG_PATH_C = "../../resources/stolarC.png";
    private const int IMG_WIDTH = 50;
    private const int IMG_HEIGHT = 50;
    #endregion // Constants

    #region Properties
    public int ID { get; set; }
    public StolarType Type { get; set; }
    public MontazneMiesto MontazneMiesto { get; set; }
    public bool Obsadeny { get; set; }
    public Workload Workload { get; set; }
    public AnimImageItem AnimImageItem { get; set; }
    #endregion // Properties

    #region Constructor
    public Stolar(StolarType stolarType, int id)
    {
        CheckImages();

        ID = _poradie++;
		Type = stolarType;
        MontazneMiesto = null!;
        Obsadeny = false;
        Workload = new Workload();

        switch (stolarType)
        {
            case StolarType.A:
                AnimImageItem = new AnimImageItem(IMG_PATH_A, IMG_WIDTH, IMG_HEIGHT);
                break;
            case StolarType.B:
                AnimImageItem = new AnimImageItem(IMG_PATH_B, IMG_WIDTH, IMG_HEIGHT);
                break;
            case StolarType.C:
                AnimImageItem = new AnimImageItem(IMG_PATH_C, IMG_WIDTH, IMG_HEIGHT);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(stolarType), stolarType, null);
        }
        AnimImageItem.SetLabel($"{ID}");
        AnimImageItem.SetPosition(Sklad.GetRandomSkladPosX(), Sklad.GetRandomSkladPosY());
        AnimImageItem.SetZIndex(2);
    }
	#endregion // Constructor

	#region Public functions
	public static void ResetPoradie()
	{
		_poradie = 1;
	}
    #endregion // Public functions

    #region Private functions
    private void CheckImages()
    {
        if (File.Exists(IMG_PATH_A) == false)
            throw new FileNotFoundException($"File {IMG_PATH_A} not found");
        if (File.Exists(IMG_PATH_B) == false)
            throw new FileNotFoundException($"File {IMG_PATH_B} not found");
        if (File.Exists(IMG_PATH_C) == false)
            throw new FileNotFoundException($"File {IMG_PATH_C} not found");
    }
    #endregion // Private functions
}

public enum StolarType
{
    Unknown,
    A,
    B,
    C
}