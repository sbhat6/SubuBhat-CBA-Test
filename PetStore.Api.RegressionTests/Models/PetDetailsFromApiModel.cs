using System.Collections.Generic;

namespace PetStore.Api.RegressionTests.Models
{
    public class PetDetailsFromApiModel
    {
        public long Id { get; set; }
        public Category Category { get; set; }
        public string Name { get; set; }
        public List<string> PhotoUrls { get; set; }
        public List<Tag> Tags { get; set; }
        public string Status { get; set; }
    }

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}