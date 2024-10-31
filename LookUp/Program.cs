using System.Net;

FileSystemWatcher spy = new()
{
    Path = @"C:\Lookup" ,
    Filter = @"*.lookup" ,
    NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.CreationTime ,
    EnableRaisingEvents = true
};

spy.Created += new FileSystemEventHandler( OnFileCreated );

Console.WriteLine( "Watching for .lookup files. Press enter to exit." );
Console.ReadLine();

static void OnFileCreated( object source , FileSystemEventArgs e )
{
    string [] lines = File.ReadAllLines( e.FullPath );
    string resolvedPath = e.FullPath.Replace( ".lookup" , ".resolved" );

    using ( StreamWriter writer = new StreamWriter( resolvedPath , false ) )
    {
        foreach ( string line in lines )
        {
            string [] fqdn = line.Split( '.' );

            if ( fqdn.Length < 3 )
            {
                writer.WriteLine( $"# {line} (invalid FQDN)" );
                continue;
            }

            writer.WriteLine( $"# {line}" );

            IPAddress [] adresses = Dns.GetHostAddresses( line );

            foreach ( IPAddress address in adresses )
                writer.WriteLine( address.ToString() );

            writer.WriteLine();
        }
    }

    Console.WriteLine( $"Processed file: {e.Name}" );
}