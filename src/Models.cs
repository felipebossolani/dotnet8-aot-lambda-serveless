namespace Bossolani.Products;

public record Product(string Id, string Name, decimal Price)
{
    public override string ToString()
    {
        return $"Product(id='{Id}', name={Name}, price={Price:n2}";
    }
}



public class ProductNotFoundException : Exception
{

}