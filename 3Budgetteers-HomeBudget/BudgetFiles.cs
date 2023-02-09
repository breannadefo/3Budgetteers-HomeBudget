using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

// ============================================================================
// (c) Sandy Bultena 2018
// * Released under the GNU General Public License
// ============================================================================

namespace Budget
{

    //<summary>
    // BudgetFiles class is used to manage the files used in the Budget project
    //</summary>

    /// <summary>
    /// Checks or returns a filepath of a file that can be read or written to. It includes
    /// default information about a filepath and methods to verify the ability to read or write
    /// to a provided file. Both methods return a working filepath. 
    /// 
    /// Everything in this class is static, so to access either of the public methods
    /// write the class name follows by a period and the method name.
    /// </summary>
    public class BudgetFiles
    {
        private static String DefaultSavePath = @"Budget\";
        private static String DefaultAppData = @"%USERPROFILE%\AppData\Local\";

        // ====================================================================
        // verify that the name of the file exists, or set the default file, and 
        // is it readable?
        // throws System.IO.FileNotFoundException if file does not exist
        // ====================================================================

        /// <summary>
        /// Checks to make sure a file exists and returns a valid filepath. It is called before 
        /// reading from a file, so it is necessary that the specified file exists. If the filepath
        /// isn't specified, it gets the filepath from the class's default filepaths. If the file 
        /// doesn't exist, a FileNotFoundException is thrown.
        /// <example>
        /// 
        /// Here is an example of how to call these methods:
        /// <code>
        /// string filepath = "../exampleFile.txt";
        /// string fileName = "exampleFile.txt";
        /// 
        /// string validFilepath = BudgetFiles.VerifyReadFromFileName(filepath, fileName);
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="FilePath">The filepath of the file that is being verified.</param> 
        /// <param name="DefaultFileName">The file name of the file that is being verified.</param> 
        /// <returns>A valid filepath.</returns> 
        /// <exception cref="FileNotFoundException">Thrown when the specified file does not exist.</exception>
        public static String VerifyReadFromFileName(String FilePath, String DefaultFileName)
        {

            // ---------------------------------------------------------------
            // if file path is not defined, use the default one in AppData
            // ---------------------------------------------------------------
            if (FilePath == null)
            {
                FilePath = Environment.ExpandEnvironmentVariables(DefaultAppData + DefaultSavePath + DefaultFileName);
            }

            // ---------------------------------------------------------------
            // does FilePath exist?
            // ---------------------------------------------------------------
            if (!File.Exists(FilePath))
            {
                throw new FileNotFoundException("ReadFromFileException: FilePath (" + FilePath + ") does not exist");
            }

            // ----------------------------------------------------------------
            // valid path
            // ----------------------------------------------------------------
            return FilePath;

        }

        // ====================================================================
        // verify that the name of the file exists, or set the default file, and 
        // is it writable
        // ====================================================================


        /// <summary>
        /// Checks if it's possible to write to a file. It is called to make sure the file can be
        /// written to before the program actually tries to write to the file. It can also be used
        /// to get a filepath of a file if the program didn't already have one. If the provided 
        /// filepath is null, it creates the filepath and possibly a directory based on the class's
        /// default filepaths. An exception is thrown if the final filepath doesn't exist or if the
        /// file cannot be written to.
        /// 
        /// <example>
        /// Here is an example of how to call these methods:
        /// <code>
        /// string filepath = "../testing.txt";
        /// string fileName = "testing.txt";
        /// 
        /// string validFilepath = BudgetFiles.VerifyWriteToFileName(filepath, fileName);
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="FilePath">The filepath of the file that is being verified.</param>
        /// <param name="DefaultFileName">The file name of the file that is being verified.</param>
        /// <returns>A valid filepath.</returns>
        /// <exception cref="Exception">Thrown when filepath is incorrect or when the file cannot be edited.</exception>
        public static String VerifyWriteToFileName(String FilePath, String DefaultFileName)
        {
            // ---------------------------------------------------------------
            // if the directory for the path was not specified, then use standard application data
            // directory
            // ---------------------------------------------------------------
            if (FilePath == null)
            {
                // create the default appdata directory if it does not already exist
                String tmp = Environment.ExpandEnvironmentVariables(DefaultAppData);
                if (!Directory.Exists(tmp))
                {
                    Directory.CreateDirectory(tmp);
                }

                // create the default Budget directory in the appdirectory if it does not already exist
                tmp = Environment.ExpandEnvironmentVariables(DefaultAppData + DefaultSavePath);
                if (!Directory.Exists(tmp))
                {
                    Directory.CreateDirectory(tmp);
                }

                FilePath = Environment.ExpandEnvironmentVariables(DefaultAppData + DefaultSavePath + DefaultFileName);
            }

            // ---------------------------------------------------------------
            // does directory where you want to save the file exist?
            // ... this is possible if the user is specifying the file path
            // ---------------------------------------------------------------
            String folder = Path.GetDirectoryName(FilePath);
            String delme = Path.GetFullPath(FilePath);
            if (!Directory.Exists(folder))
            {
                throw new Exception("SaveToFileException: FilePath (" + FilePath + ") does not exist");
            }

            // ---------------------------------------------------------------
            // can we write to it?
            // ---------------------------------------------------------------
            if (File.Exists(FilePath))
            {
                FileAttributes fileAttr = File.GetAttributes(FilePath);
                if ((fileAttr & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                {
                    throw new Exception("SaveToFileException:  FilePath(" + FilePath + ") is read only");
                }
            }

            // ---------------------------------------------------------------
            // valid file path
            // ---------------------------------------------------------------
            return FilePath;

        }



    }
}
