namespace FileApi.Persistances
{
    using FileApi.Models;
    using System.Linq;

    public class FilesPersistance
    {
        #region Get
        public List<string> GetListOfFileNames()
        {
            string directoryPath = "Files";
            List<string> files = Directory.GetFiles(directoryPath).ToList();

            return files;
        }

        public async Task<FileData?> GetFileAsync(string fileName, string fileExtension)
        {
            string folderPath = "Files";

            if (Directory.Exists(folderPath))
            {
                string filePath = Path.Combine(folderPath, fileName + fileExtension);
                if (File.Exists(filePath))
                {
                    byte[] bytes = await File.ReadAllBytesAsync(filePath);
                    return new FileData(bytes, fileExtensinToMimeType(fileExtension), fileName);
                }
            }
            return null;
        }
        #endregion

        #region Upload
        public async Task SaveFilesAsync(List<IFormFile> files)
        {
            foreach (IFormFile file in files)
            {
                await SaveFileAsync(file);
            }
        }
        public async Task SaveFileAsync(IFormFile file)
        {
            string fileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}{Path.GetExtension(file.FileName.ToLower())}";
            string folderPath = "Files";

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string fileRoute = Path.Combine(folderPath, fileName);

            using (FileStream fileStream = File.Create(fileRoute))
            {
                await file.OpenReadStream().CopyToAsync(fileStream);
            }
        }

        #endregion

        #region related to file extension
        public bool IsAcceptableFileExtension(string fileExtension)
        {
            #region variables
            List<string> fileExtensions = new(){ ".png", ".jpg", ".jpeg", ".txt", "rtf", ".gif"};
            #endregion
            return fileExtensions.Contains(fileExtension.ToLower());
        }

        public string fileExtensinToMimeType(string fileExtension)
        {
            #region variables
            Dictionary<string, string> fileExtensions = new Dictionary<string, string>();
            fileExtensions.Add(".png", "image/png");
            fileExtensions.Add(".jpg", "image/jpeg");
            fileExtensions.Add(".jpeg", "image/jpeg");
            fileExtensions.Add(".txt", "text/plain");
            fileExtensions.Add(".rtf", "application/rtf");
            fileExtensions.Add(".gif", "image/gif");
            #endregion
            if (IsAcceptableFileExtension(fileExtension))
            {
                bool extensionInList = fileExtensions.ContainsKey(fileExtension.ToLower());

                #pragma warning disable CS8603 // Possible null reference return.
                return extensionInList ? fileExtensions.GetValueOrDefault(fileExtension.ToLower()) : "not found";
                #pragma warning restore CS8603 // Possible null reference return.
            }
            return "It is not an acceptable file extension.";
        }
        #endregion
    }
}
