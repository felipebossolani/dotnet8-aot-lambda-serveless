using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bossolani.Products;

internal class Database
{
    private List<Product> products;
    public Database()
    {
        products = new()
        {
            new("SKU-001", "Macbook Pro", 1299.99M),
            new("SKU-002", "Iphone 15", 799.99M),
            new("SKU-003", "Samsung S24", 999.99M),
            new("SKU-004", "Ipad PRO 11", 899.99M),
        };
    }

    public async Task<Product> GetProduct(string id) => products.FirstOrDefault(x => x.Id == id);

    public async Task<List<Product>> GetAllProducts() => products;

    public async Task<Product> PutProduct(Product updatedProduct)
    {
        var product = products.FirstOrDefault(x => x.Id == updatedProduct.Id);
        /*
        Aqui o produto é readonly por ser um record em memória por isso as linhas abaixo não compilam
        product.Name = updatedProduct.Name;
        product.Price = updatedProduct.Price;
        */
        return product with { Name = updatedProduct.Name, Price = updatedProduct.Price };
    }

    public async Task DeleteProduct(string id)
    {
        var product = products.FirstOrDefault(x => x.Id == id);
        if (product is null)
            throw new ProductNotFoundException();

        products.Remove(product);
    }
}
