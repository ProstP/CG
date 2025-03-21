using Task1.Figure;
using Task1.FigureImp;

namespace Task1;

internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    [Obsolete]
    static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        Form1 form1 = new Form1();
        form1.AddFigure(new RhombicTruncatedCuboctahedron(0.3f, 0.1f));
        Application.Run(form1);
    }
}