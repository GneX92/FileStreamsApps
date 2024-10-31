using System.Windows.Forms;

namespace FileWriter;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        string? path;

        OpenFileDialog openFile = new();

        while ( true )
        {
            Console.Clear();

            if ( openFile.ShowDialog() == DialogResult.OK )
            {
                path = openFile.FileName;
                break;
            }
            else
            {
                Console.WriteLine( "No file selected." );
                Console.WriteLine( $"\n<Esc> Beenden\n<Backspace> Enter path\n<Enter> Open File Dialog" );
                ConsoleKeyInfo key = Console.ReadKey();

                if ( key.Key == ConsoleKey.Escape )
                    Application.Exit();

                if ( key.Key == ConsoleKey.Backspace )
                {
                    Console.Write( "\rPath: " );
                    path = Console.ReadLine();
                    break;
                }

                if ( key.Key == ConsoleKey.Enter )
                    continue;
            }
        }

        while ( true )
        {
            Console.Clear();

            string? input = Console.ReadLine();

            if ( string.IsNullOrWhiteSpace( input ) )
                continue;

            if ( input == "." )
                break;

            File.AppendAllText( path , DateTime.Now.ToLongTimeString() + " " + input + "\n" );
        }

        Console.Clear();

        foreach ( string line in File.ReadAllLines( path ) )
            Console.WriteLine( line );

        Console.ReadLine();
    }
}