using System.Windows.Media;
using OSPAnimator;

namespace DSAgentSimulationWoodwork.Entities;

public class MontazneMiesto
{
    #region Constants
    public const int HEIGHT = 100;
    public const int WIDTH = 200;
    public const int MARGIN = 50;
    #endregion // Constants

    #region Properties
    public int ID { get; set; }
    public Tovar Tovar { get; set; }
    public Stolar Stolar { get; set; }
    public int PosX { get; set; }
    public int PosY { get; set; }
    public AnimShapeItem AnimShapeItem { get; set; }
    #endregion // Properties

    #region Constructor
    public MontazneMiesto(int id)
    {
        ID = id;
        Tovar = null;
        Stolar = null;

        int posX = (id-1) / 3;
        int posY = (id-1) % 3;

        AnimShapeItem = new();
        PosX = Sklad.SKLAD_POS_X + Sklad.SKLAD_WIDTH + (WIDTH + MARGIN) * posX;
        PosY = 10 + (HEIGHT + MARGIN) * posY;
        AnimShapeItem.SetPosition(PosX, PosY);
        AnimShapeItem.Width = WIDTH;
        AnimShapeItem.Height = HEIGHT;
        AnimShapeItem.AnimShape = AnimShape.RECTANGLE;
        AnimShapeItem.Color = Colors.Gray;
        AnimShapeItem.Fill = true;
    }
    #endregion // Constructor
    
    #region Public functions
    public override string ToString()
    {
        bool obsadeny = false;
        int stolarId = -1;
        StolarType stolarType = StolarType.Unknown;
        int objednavkaId = -1;
        TovarType tovarType = TovarType.Unknown;
        TovarStatus tovarStatus = TovarStatus.Unknown;
        int vytazenost = -1;
        int tovarPoradie = -1;
        if (Stolar is not null) obsadeny = Stolar.Obsadeny;
        if (Stolar is not null) stolarType = Stolar.Type;
        if (Stolar is not null) stolarId = Stolar.ID;
        if (Stolar is not null) vytazenost = (int)Stolar.Workload.GetValue();
        if (Tovar is not null) objednavkaId = Tovar.ObjednavkaId;
        if (Tovar is not null) tovarType = Tovar.Type;
        if (Tovar is not null) tovarStatus = Tovar.Status;
        if (Tovar is not null) tovarPoradie = Tovar.Poradie;
        if (Tovar is null && Stolar is null) return $"Miesto: {ID}";
        if (Tovar is null && Stolar is not null) return $"Miesto: {ID},\t Stolar: {stolarType}{stolarId} {obsadeny} {vytazenost}%";
        if (Tovar is not null && Stolar is null) return $"Miesto: {ID},\t Tovar: {objednavkaId}.{tovarPoradie} {tovarType}, {tovarStatus}";
        return $"Miesto: {ID},\t Tovar: {objednavkaId}.{tovarPoradie} {tovarType}, {tovarStatus},\t Stolar: {stolarType}{stolarId} {obsadeny} {vytazenost}%";
    }
    #endregion // Public functions
}