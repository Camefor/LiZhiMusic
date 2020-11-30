using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Music.Models {
    public class SourceModel {

        /// <summary>
        /// 歌名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 歌手
        /// </summary>
        public string author { get; set; }

        /// <summary>
        /// 音乐文件
        /// </summary>
        public string src { get; set; }

        /// <summary>
        /// 封面图片
        /// </summary>
        public string cover { get; set; }
        public int count { get; set; }
    }

    public class FileInfoModel {
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Extension { get; set; }
        public string MB { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastAccessTime { get; set; }
    }
}
