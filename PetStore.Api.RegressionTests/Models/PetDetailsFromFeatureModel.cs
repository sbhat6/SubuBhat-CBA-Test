using System;
namespace PetStore.Api.RegressionTests.Models
{
    public class PetDetailsFromFeatureModel
    {
        public long Id { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Name { get; set; }
        public string PhotoUrls { get; set; }
        public int TagsId { get; set; }
        public string TagsName { get; set; }
        public string Status { get; set; }
    }
}
