using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary
{
    internal class FileMetadata
    {
        public string FileId { get; set; }
        public string FileName { get; set; }
        public long Size { get; set; }
        public string OwnerUid { get; set; }
        public string StoragePath { get; set; }
        public string Path { get; set; }
        public string UploadedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
