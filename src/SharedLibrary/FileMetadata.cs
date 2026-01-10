using Google.Cloud.Firestore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary
{
    [FirestoreData]
    public class FileMetadata
    {
        [FirestoreProperty]
        [JsonProperty("fileId")]
        public string FileId { get; set; }
        [FirestoreProperty]
        [JsonProperty("fileName")]
        public string FileName { get; set; }
        [FirestoreProperty]
        [JsonProperty("size")]
        public long Size { get; set; }
        [FirestoreProperty]
        [JsonProperty("ownerUid")]
        public string OwnerUid { get; set; }
        [FirestoreProperty]
        [JsonProperty("storagePath")]
        public string StoragePath { get; set; }
        [FirestoreProperty]
        [JsonProperty("path")]
        public string Path { get; set; }
        [FirestoreProperty]
        [JsonProperty("uploadedDate")]
        public string UploadedDate { get; set; }
        [FirestoreProperty]
        [JsonProperty("isDeleted")]
        public bool IsDeleted { get; set; }

        [FirestoreProperty]
        [JsonProperty("isStarred")] // Để đồng bộ tên với JSON/Firestore
        public bool IsStarred { get; set; }


    }
}
