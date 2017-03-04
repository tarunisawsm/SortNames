using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace GlobalX
{

    /// <summary>
    /// Class SortFileContent which reads content from file, sorts it and writes it into new file
    /// </summary>
    class SortFileContent
    {
        private string inputTextFile = null;
        private string inputTextFileName = null;
        private string inputTextFileLocation = null;

        /*
         * 
        //Method to get the input file name, path and full file path.
        *
        */
        public bool setInputFilePath( string filePath )
        {
            bool result = true;
            inputTextFile = Path.GetFullPath( filePath );
            var fileInfo = new FileInfo( inputTextFile );

            //Check if the file exists or if it is empty
            if ( !fileInfo.Exists )
            {
                Console.WriteLine( "File name does not exist" );
                result = false;
            }
            else if( ( 0 == fileInfo.Length ) )
            {
                Console.WriteLine( "Empty file passed." );
                result = false;
            }
            else
            {
                inputTextFileName = Path.GetFileNameWithoutExtension( filePath );
                inputTextFileLocation = Path.GetDirectoryName( filePath );
            }
            return result;
        }
        /*
         * 
        //Method to sort the names.
        *
        */
        public IEnumerable<string> sortFileContent( ref bool sortResult )
        {
            IEnumerable<string> sortedNames = null;
            bool formatFlag = false;

            foreach ( string line in File.ReadAllLines( inputTextFile ) )
            {
                if ( ( Regex.IsMatch( line, "[a-zA-Z]+[, ]+[a-zA-Z]+") ) )
                {
                    formatFlag = true;
                }
                else
                {
                    Console.WriteLine( "Invalid data in file" );
                    break;
                }
            }
            if ( true == formatFlag )
            {
                sortedNames = File.ReadLines( inputTextFile )
                                              .Select( entry => entry.Split( ',' ).ToArray() )
                                              .OrderBy( entry => entry[ 0 ] )
                                              .ThenBy( entry => entry[ 1 ] )
                                              .Select( entry => string.Join( ",", entry ) );
                sortResult = true;
            }
            return sortedNames;
        }

        /*
         * 
        //Method to write the final sorted data to output file.
        *
        */
        public void writeOutputFileData( IEnumerable<string> result )
        {
            //Create a new file name with name as required
            string outputTextFile = inputTextFileLocation + inputTextFileName + "-sorted.txt";
            //Write sorted data to the new file with the name as above
            foreach( var row in result )
            {
                Console.WriteLine( row );
            }
            File.WriteAllLines( outputTextFile, result );
            Console.WriteLine( "Finished: created {0}", outputTextFile );
        }

    }


    /// <summary>
    /// Mail function class
    /// </summary>
    class Program
    {
        static void Main( string[] args )
        {
            if ( 0 < args.Length )
            {
                bool result = false;
                bool sortResult = false;
                IEnumerable<string> sortedString;
                SortFileContent sortNames = new SortFileContent();
                result = sortNames.setInputFilePath( args[0] );

                if ( false == result )
                {
                    Console.WriteLine("Main function: File opening failed.");
                }
                else
                {
                    sortedString = sortNames.sortFileContent( ref sortResult );
                    if (false == sortResult)
                    {
                        Console.WriteLine("Main function: Sort failed");
                    }
                    else
                    {
                        sortNames.writeOutputFileData(sortedString);
                    }
                }
            }
            else
            {
                Console.WriteLine("Main function: No arguments passed");
            }
        }
    }
}