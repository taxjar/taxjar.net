using System.Reflection;
using System.Text.RegularExpressions;

namespace Taxjar.Tests.Infrastructure;

public static class TaxjarFixture
{
    private const string manifestResourceNamePrefix = "Taxjar.Tests.Fixtures.";
    private static Regex manifestResourcePathRegEx = new Regex(@"\.(?=.*?\.)", RegexOptions.Compiled);
    private static Assembly currentAssembly = GetCurrentAssembly();
    private static Dictionary<string, string> testDataFiles = new Dictionary<string, string>();
    public static Assembly GetCurrentAssembly()
    {
        var foo =  currentAssembly = currentAssembly ?? Assembly.GetExecutingAssembly();
        return foo;
    }

    public static Dictionary<string, string> GetTestDataFiles() => GetCurrentAssembly()
        .GetManifestResourceNames()
        .ToDictionary(key => {
            var truncatedKey = key.Replace(manifestResourceNamePrefix, string.Empty, StringComparison.OrdinalIgnoreCase);
            return manifestResourcePathRegEx.IsMatch(truncatedKey) ? manifestResourcePathRegEx.Replace(truncatedKey,"/") : truncatedKey;
        }, val => val, StringComparer.OrdinalIgnoreCase);

    public static Stream? GetResourceStream(string resourceName)
    {
        if (testDataFiles.Count == 0)
            testDataFiles = GetTestDataFiles();

        if (!testDataFiles.ContainsKey(resourceName))
            throw new FileNotFoundException($"{resourceName} is not a valid resource", resourceName);

        Stream? stream = GetCurrentAssembly().GetManifestResourceStream(testDataFiles[resourceName]);

        return stream;
    }

    public static string GetJSON(string fixturePath)
    {
        using (var stream = GetResourceStream(fixturePath))
        {
            if(stream is null)
            {
                throw new NullReferenceException($"{fixturePath} is not a valid resource, or failed to open.");
            }

            stream!.Position = 0;
            using (var reader = new StreamReader(stream))
                return reader.ReadToEnd();
        }
    }
}