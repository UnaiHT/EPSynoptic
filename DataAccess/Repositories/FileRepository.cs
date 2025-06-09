using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.DataContext;
using File = Domain.Models.File;

namespace DataAccess.Repositories
{
    public class FileRepository
    {
        private FileDbContext _fileContext;

        public FileRepository(FileDbContext fileContext)
        {
            _fileContext = fileContext;
        }

        public IQueryable<File> GetFiles()
        {
            return _fileContext.Files;
        }

        public void AddFile(File f)
        {
            f.Date = DateTime.Now;
            _fileContext.Files.Add(f);
            _fileContext.SaveChanges();
        }
    }
}
