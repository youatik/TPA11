using System;
namespace TPA11.Models
{
    public class LibraryItemResult
    {
        public long ean_isbn13 { get; set; }
        public string Title { get; set; }
        public string Creators { get; set; }
        public string Description { get; set; }
        // Add properties for other columns as needed
    }

}

