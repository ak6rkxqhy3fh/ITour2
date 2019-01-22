using System;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace Touroperators
{
    public class OpenData
    {

        private static readonly HttpClient httpClient = new HttpClient();

        public string Property { get; set; } // Код регионального списка туроператоров
        public string Title { get; set; }    // Внятное название
        public string PassportUri { get; set; }    // URI паспорта региона
        public string Format { get; set; }   // Формат файла (JSON)

        Passport Passport { get; set; }

        string RegionDataString { get; set; }

        public async Task<string> GetRegionStringAsync()
        {
            await LoadPassportAsync();

            try
            {
                RegionDataString = await httpClient.GetStringAsync(Passport.Data[1].Source);
            }
            catch (Exception)
            {
                throw;                
            }
            return RegionDataString;
        }

        private async Task LoadPassportAsync()
        {
            try
            {
                // Сериализатор Паспорта региона - метаданные содержащие URI структуры данных и URI данных для каждого региона
                DataContractJsonSerializer passportSerializer = new DataContractJsonSerializer(typeof(Passport));

                // Получить стрим с json метаданными для паспорта очередного региона
                Stream passportStream = await httpClient.GetStreamAsync(PassportUri);

                // Десериализовать json в объект registryPassport
                Passport = passportSerializer.ReadObject(passportStream) as Passport;
            }
            catch (Exception)
            {
                throw;
            }
        }


    }

}
