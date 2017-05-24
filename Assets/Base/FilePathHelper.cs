using System;
using System.IO;

namespace Keedy.Base.IO
{
    public static class FilePathHelper
    {
        /// <summary>
        /// 获取文件所在目录
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetDirectoryName(string path)
        {
            return Path.GetDirectoryName(path);
        }
        /// <summary>
        /// 获取文件后缀名
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetFileExtension(string path)
        {
            return Path.GetExtension(path);
        }
        /// <summary>
        /// 获取文件名（包含后缀名）
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetFileName(string path)
        {
            return Path.GetFileName(path);
        }
        /// <summary>
        /// 获取文件名（不包含后缀）
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetFileNameWithoutExtension(string path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }
        /// <summary>
        /// 替换文件后缀名
        /// </summary>
        /// <param name="path"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static string ReplaceFileExtension(string path, string extension)
        {
            return Path.ChangeExtension(path, extension);
        }
        /// <summary>
        /// 路径格式化，把\\替换为/
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string PathNormalize(this string path)
        {
            return path.Replace("\\", "/");
        }        
    }

}
