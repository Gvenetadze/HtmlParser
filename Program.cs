using HtmlParser;
using Newtonsoft.Json;

var fileName = "data.html";
var path = Path.Combine(Environment.CurrentDirectory, fileName);
var htmlData = File.ReadAllText(path);

var parser = new Parser(htmlData);
var products = parser.ParseProducts();

var result = JsonConvert.SerializeObject(products, Formatting.Indented);

Console.WriteLine(result);