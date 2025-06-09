using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.DataContext;

namespace DataAccess.Repositories
{
    public class FileRepository
    {
        private FileDbContext _fileContext;

        public FileRepository(FileDbContext fileContext)
        {
            _fileContext = fileContext;
        }

        
    }
}
