using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.FileRepository.Models
{
    public class FileStorageOptions
    {
        public string? UserRepositoryPath { get; set; }
        public string? ProjectRepositoryPath { get; set; }
        public string? DomainTaskRepositoryPath { get; set; }
    }
}
