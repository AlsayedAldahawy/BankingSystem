using BankingSystem.Dtos;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BankingSystem.Utilities
{

    public class IdStorage
    {
        public static int ReadJsonValue(string key, string filePath="Utilities/savedData.json")
        {
            try
            {
                var jsonData = File.ReadAllText(filePath);
                var jsonObject = JObject.Parse(jsonData);

                if (jsonObject[key] is JValue value && value.Value != null) 
                { 
                    return (int)value; 
                }

                return 9;
        }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading from JSON file: {ex.Message}");
                return 7;
            }
}

        public static void WriteJsonValue(string key, int value, string filePath = "Utilities/savedData.json") 
        { 
            try {
                var jsonData = File.ReadAllText(filePath); 
                var jsonObject = JObject.Parse(jsonData); 

                jsonObject[key] = value; 

                File.WriteAllText(filePath, jsonObject.ToString()); 
            } 
            catch (Exception ex) { 
                Console.WriteLine($"Error writing to JSON file: {ex.Message}"); 
            } 
        }
    }


}
