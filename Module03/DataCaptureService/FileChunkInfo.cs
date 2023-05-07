namespace DataCaptureService
{
    public class FileChunkInfo
    {
        public string Id { get; set; }
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public decimal ChunkNumber { get; set; }
        public decimal TotalChunks { get; set; }
        public byte[] ChunkData { get; set; }
    }
}