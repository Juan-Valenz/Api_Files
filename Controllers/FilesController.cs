using FileApi.Persistances;
using FileApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.IO;


namespace FileApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilesController : ControllerBase
    {

        private FilesPersistance filesPersistance = new ();


        [HttpGet("FileNamesList")]
        public ActionResult<List<IFormFile>> Get()
        {
            List<string> files = filesPersistance.GetListOfFileNames();

            return Ok(files);
        }


        [HttpGet("File/{fileName}/{fileExtension}")]
        public async Task<ActionResult<IFormFile>> Get(string fileName, string fileExtension)
        {
            FileData? file = await filesPersistance.GetFileAsync(fileName, fileExtension);

            if (file != null)
            {
                return File(file.fileContent, file.contentType, file.fileDownloadName);
            }
            return NoContent();
        }


        [HttpPost]
        public async Task<ActionResult> Post(IFormFile file)
        {
            long fileSize = file.Length;
            string fileExtension = Path.GetExtension(file.FileName);
            if (!(fileSize > 0))
            {
                return Conflict("The files has no content.");
            }
            else if (fileSize > 4000000)
            {
                return Conflict("The files is too long, the file needs to be 4 megabytes of shorter.");
            }
            else if (!filesPersistance.IsAcceptableFileExtension(fileExtension))
            {
                return Conflict("Not acceptable file extension. Acceptable file extensions are .txt, .rtf, .jpg, .jpeg, .png and .gif.");
            }

            await filesPersistance.SaveFileAsync(file);

            return Ok();
        }
    }
}