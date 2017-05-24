using System;
using System.IO;

namespace Keedy.Base.IO
{
    public static class DirectoryHelper
    {
        public static bool Exists(string directoryPath)
        {
            return Directory.Exists(directoryPath);
        }
        public static bool CreateDirectory(string directoryPath)
        {
            try
            {
                Directory.CreateDirectory(directoryPath);
                return true;
            }
            catch(Exception ex)
            {
#if TEST_CDOE
                UnityEngine.Debug.LogError(ex.Message);
#endif
            }
            return false;
        }
        public static bool CreateDirIfNotExist(string directoryPath)
        {
            if (Directory.Exists(directoryPath) == false)
            {
                return CreateDirectory(directoryPath);
            }
            return true;
        }

        public static bool DeleteDirectory(string directoryPath, bool recursive = true)
        {
            if (Exists(directoryPath))
            {
                try
                {
                    Directory.Delete(directoryPath, recursive);
                    return true;
                }
                catch (System.Exception ex)
                {
#if TEST_CODE
                    UnityEngine.Debug.LogError(ex.Message);
#endif
                }
                return false;
            }
            return true;
        }
        public static bool GetFileNames(string directoryPath, out string[] fileList)
        {
            fileList = null;
            if (Exists(directoryPath))
            {
                try
                {
                    fileList = Directory.GetFiles(directoryPath);
                    return true;
                }
                catch (System.Exception ex)
                {
#if TEST_CODE
                    UnityEngine.Debug.LogError(ex.Message);
#endif
                }
            }
            return false;
        }

        public static bool GetDirectories(string directoryPath, out string[] directoryList)
        {
            directoryList = null;
            if (Exists(directoryPath))
            {
                try
                {
                    directoryList = Directory.GetDirectories(directoryPath);
                    return true;
                }
                catch (System.Exception ex)
                {
#if TEST_CODE
                    UnityEngine.Debug.LogError(ex.Message);
#endif
                }
            }
            return false;
        }
    }
}
