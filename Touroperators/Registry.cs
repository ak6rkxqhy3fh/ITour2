using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Touroperators
{
    public class Registry
    {
        private static readonly HttpClient httpClient = new HttpClient();

        public Registry()
        {
            OpenDataList = new List<OpenData>();
        }

        public List<OpenData> OpenDataList { get; set; }

        public async Task<List<OpenData>> LoadOpenDataAsync(string openDataUri)
        {            
                string opendataString;

                try
                {
                    opendataString = await httpClient.GetStringAsync(openDataUri);
                }
                catch (Exception)
                {
                    throw;
                }

                List<string> opendataStringList = opendataString.Split("\n").ToList();       
                opendataStringList.RemoveRange(0, 2);                                        
                opendataStringList.RemoveAt(opendataStringList.Count - 1);  
                
                foreach (string str in opendataStringList)
                {
                    string[] values = (str).Split(','); 
                OpenData openData = new OpenData
                {
                    Property = values[0],        
                    Title = values[1],          
                    PassportUri = values[2],     
                    Format = values[3]          
                };

                OpenDataList.Add(openData);  
                }                
            
            return OpenDataList;
        }
    }
}
