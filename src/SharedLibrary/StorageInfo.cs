namespace SharedLibrary
{
    public class StorageInfo
    {
        public long TotalUsed { get; set; }           // Dung lượng đã sử dụng (bytes)
        public long MaxQuota { get; set; }            // Giới hạn tối đa (bytes)
        public long TotalRemaining { get; set; }      // Dung lượng còn trống (bytes)
        public int UsagePercent { get; set; }         // Phần trăm sử dụng (0-100)
    }
}
