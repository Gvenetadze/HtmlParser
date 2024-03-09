using HtmlAgilityPack;
using System.Globalization;
using System.Net;

namespace HtmlParser;

internal class Parser(string htmlData)
{
    public string HtmlData { get; set; } = htmlData;

    public List<Product> ParseProducts()
    {
        var document = new HtmlDocument();

        document.LoadHtml(htmlData);

        var products = new List<Product>();
        var items = document.DocumentNode.SelectNodes("//div[@class='item']");

        foreach (var item in items)
        {
            var productName = GetProductName(item);
            var price = GetPrice(item);
            var rating = GetRating(item);

            products.Add(new Product 
            { 
                ProductName = productName, 
                Price = price, 
                Rating = rating
            });
        }

        return products;
    }

    #region Private Methods
    private string GetProductName(HtmlNode html)
    {
        return WebUtility
            .HtmlDecode(html.SelectSingleNode(".//img")
            .GetAttributeValue("alt", ""));
    }

    private string GetPrice(HtmlNode html)
    {
        return
            html
            .SelectSingleNode(".//span[contains(@style, 'display: none')]")
            .InnerText
            .Trim()
            .Replace("$", "")
            .Replace(",", "");
    }

    private string GetRating(HtmlNode html)
    {
        var ratingString = html.GetAttributeValue("rating", "");
        var rating = Convert.ToDouble(ratingString, CultureInfo.InvariantCulture);

        if (rating > 5)
        {
            rating /= 2;
        }

        var result = rating.ToString("0.0", CultureInfo.InvariantCulture);

        if (rating % 1 == 0)
        {
            result = rating.ToString("0", CultureInfo.InvariantCulture);
        }

        return result;
    } 
    #endregion
}
