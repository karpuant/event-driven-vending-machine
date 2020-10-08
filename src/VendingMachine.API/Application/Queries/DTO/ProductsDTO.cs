using System.Collections.Generic;

namespace VendingMachine.API.Application.Queries.DTO
{
    public class ProductsDTO
    {
        public List<ProductDTO> Products { get; set; } = new List<ProductDTO>();
    }
}
