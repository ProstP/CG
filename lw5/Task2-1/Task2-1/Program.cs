using Task2_1.ModelView;

namespace Task2_1;

internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        ModelImp.Model model = new();
        Form1 form1 = new();
        ViewModel viewModel = new(model, form1);

        Application.Run(form1);
    }
}