using System;
namespace TPA11.Models
{
    public class LibraryItemResult //classe pour contenir les resultat du type LibraryItem
    {
        public long ean_isbn13 { get; set; }
        public string Title { get; set; }
        public string Creators { get; set; }
        public string Description { get; set; }
        
    }

}

