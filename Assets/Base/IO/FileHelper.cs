using System.IO;
using System.Collections.Generic;
using System.Text;
using System;


namespace Keedy.Base.IO
{
    public static class FileHelper
    {
        /// <summary>
        /// 查看指定路径的文件是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool Exists(string path)
        {
            return File.Exists(path);
        }

        /// <summary>
        /// 读取指定文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="content">文件内容</param>
        /// <param name="encoding">编码格式（默认为UTF8）</param>
        /// <returns></returns>
        public static bool LoadFile(string path, out string content, Encoding encoding = null)
        {
            content = string.Empty;
            if(FileHelper.Exists(path))
            {
                try
                {
                    Encoding myEncoding = encoding == null ? Encoding.UTF8 : encoding;
                    using (StreamReader sr = new StreamReader(path, myEncoding))
                    {
                        content = sr.ReadToEnd();
                        return true;
                    }
                }
                catch(Exception ex)
                {
#if TEST_CODE
                    UnityEngine.Debug.LogError("ReadFile Failed:" + ex.Message);
#endif
                }
            }
            {
#if TEST_CODE
                UnityEngine.Debug.LogError("Can't Find File:"+path);
#endif
            }
            return false;
        }
        /// <summary>
        /// 读取指定文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="content">文件内容</param>
        /// <returns></returns>
        public static bool LoadByteFile(string path, out byte[] content)
        {
            content = null;
            if(File.Exists(path))
            {
                try
                {
                    using(FileStream fs = File.OpenRead(path))
                    {
                        content = new byte[fs.Length];
                        using(BinaryReader br = new BinaryReader(fs))
                        {
                            br.Read(content, 0, (int)fs.Length);
                        }
                    }
                    return true;
                }
                catch(Exception ex)
                {
#if TEST_CODE
                    UnityEngine.Debug.LogError("LoadByteFile Failed:"+path+"  "+ex.Message);
#endif
                }
            }
            else
            {
#if TEST_CDOE
                UnityEngine.Debug.LogError("Can't Finde Flie:"+path);
#endif
            }
            return false;
        }

        public static bool SaveByteFile(string path, byte[] content, Encoding encoding = null)
        {
            if (DirectoryHelper.CreateDirIfNotExist(FilePathHelper.GetDirectoryName(path)))
            {
                try
                {
                    Encoding myencoding = encoding == null ? Encoding.UTF8 : encoding;
                    using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
                    {
                        using(BinaryWriter sw = new BinaryWriter(fs, myencoding))
                        {
                            sw.Write(content);
                            sw.Flush();
                            return true;
                        }
                    }
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

        public static bool SaveFile(string path, string content, Encoding encoding = null)
        {
            if (DirectoryHelper.CreateDirIfNotExist(FilePathHelper.GetDirectoryName(path)))
            {
                try
                {
                    Encoding myencoding = encoding == null ? Encoding.UTF8 : encoding;
                    using (FileStream fs = new FileStream(path, FileMode.Create))
                    {
                        using (StreamWriter sw = new StreamWriter(fs, myencoding))
                        {
                            sw.Write(content);
                            sw.Flush();
                        }
                    }
                    return true;
                }
                catch (Exception ex)
                {
#if TEST_CODE
                    UnityEngine.Debug.LogError(ex.Message);
#endif
                }
            }
            return false;
        }

        public static bool CreateFile(string path)
        {
            if (Exists(path) == false)
            {
                try
                {
                    if (string.IsNullOrEmpty(path))
                    {
                        return false;
                    }
                    DirectoryHelper.CreateDirIfNotExist(FilePathHelper.GetDirectoryName(path));
                    FileStream fs = File.Create(path);
                    fs.Close();
                    return true;
                }
                catch (System.Exception ex)
                {
                    return false;
#if TEST_CODE
                    UnityEngine.Debug.LogError(ex.Message);
#endif
                }
            }
            return true;
        }

        public static bool DeleteFile(string path)
        {
            if (Exists(path))
            {
                try
                {
                    File.Delete(path);
                    return true;
                }
                catch (System.Exception ex)
                {
                    return false;
#if TEST_CODE
                    UnityEngine.Debug.LogError(ex.Message);
#endif
                }
            }
            return true;
        }
        public static bool MoveFile(string srcPath, string destPath)
        {
            if (Exists(srcPath))
            {
                if(DirectoryHelper.CreateDirIfNotExist(FilePathHelper.GetDirectoryName(destPath)))
                {
                    try
                    {
                        File.Move(srcPath, destPath);
                        return true;
                    }
                    catch (System.Exception ex)
                    {
#if TEST_CODE
                        UnityEngine.Debug.LogError(ex.Message);
#endif
                    }
                }
            }
            return false;
        }

        public static bool CopyFile(string srcPath, string destPath, bool overwirte = true)
        {
            if(Exists(srcPath))
            {
                string destDirec = FilePathHelper.GetDirectoryName(destPath);
                if (DirectoryHelper.CreateDirIfNotExist(destDirec))
                {
                    try
                    {
                        File.Copy(srcPath, destPath, overwirte);
                        return true;
                    }
                    catch (System.Exception ex)
                    {
#if TEST_CODE
                        UnityEngine.Debug.LogError(ex.Message);
#endif
                    }
                }
            }
            return false;
        }

        public static bool GetFileSize(string path, out long fileSize)
        {
            fileSize = 0;
            if (Exists(path))
            {
                try
                {
                    using (FileStream fs = new FileStream(path, FileMode.Open))
                    {
                        fileSize = fs.Length;
                    }
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
