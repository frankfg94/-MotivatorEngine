using System;
using System.IO;
using System.Reflection;

namespace MotivatorEngine
{
    public static class MotivatorGlobals
    {
        public const string ENVIRONMENT_VARIABLE_MOTIVATOR_FOLDER_PATH = "MotivatorFolderPath";
        public static string MOTIVATOR_FOLDER_PATH = Environment.GetEnvironmentVariable(ENVIRONMENT_VARIABLE_MOTIVATOR_FOLDER_PATH);

        public static void CheckDependencies()
        {
            if (String.IsNullOrEmpty(MOTIVATOR_FOLDER_PATH))
            {
                throw new ArgumentNullException($"Windows Environment variable missing : {ENVIRONMENT_VARIABLE_MOTIVATOR_FOLDER_PATH}");
            }
            else if (!Directory.Exists(MOTIVATOR_FOLDER_PATH))
            {
                throw new ArgumentNullException("The path indicated is invalid for the main motivator folder : " + MOTIVATOR_FOLDER_PATH);
            }
        }


    }
}
