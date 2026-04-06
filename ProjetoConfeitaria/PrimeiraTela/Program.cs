namespace PrimeiraTela
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
<<<<<<< HEAD
            Application.Run(new TelaLogin());
=======
           
            Application.Run(new FrmHistoricoPedidos());

>>>>>>> cb312eecb9613f9be61e0059fc76d8f181760243
        }
    }
}