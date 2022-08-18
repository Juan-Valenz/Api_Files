namespace FileApi.Models
{
    public class FileData
    {
        public Byte[] fileContent { get; set; }
        public string contentType { get; set; }
        public string? fileDownloadName { get; set; }

        public FileData(Byte[] fileContent, string contentType, string? fileDownloadName)
        {
            this.fileContent = fileContent;
            this.contentType = contentType;
            this.fileDownloadName = fileDownloadName;
        }
    }
}
