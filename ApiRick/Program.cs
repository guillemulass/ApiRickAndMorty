namespace ApiRick;

abstract class Program
{
    static async Task Main()
    {
        try
        {
            bool keepActive = true;
            while (keepActive)
            {
                Console.WriteLine("Introduzca el id del personaje del que quiera sacar la informacion: (1:826)");
                string characterId = Console.ReadLine();
                await CharacterInfoGetter(int.Parse(characterId));
                keepActive = false;
            }
        }
        catch (NullReferenceException)
        {
            Console.WriteLine("Introduzca un id valido, numero del 1 al 826 (summer tiene el id 3)");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
    
    private static async Task CharacterInfoGetter(int characterId)
    {
        
        // URL de la API
        string apiUrl = $"https://rickandmortyapi.com/api/character/{characterId}";

        // Crear una instancia de HttpClient
        using (HttpClient client = new HttpClient())
        {
            // Realizar la solicitud GET a la API
            HttpResponseMessage response = await client.GetAsync(apiUrl);

            // Leer el contenido de la respuesta como una cadena JSON
            string jsonContent = await response.Content.ReadAsStringAsync();

            // Procesar la cadena JSON for RespuestaPersonaje
            CharacterResponse? characterData =
                Newtonsoft.Json.JsonConvert.DeserializeObject<CharacterResponse>(jsonContent);

            string episodeInfo = await EpisodeGetter(characterData.Episode);

            // Muestro los datos del personaje
            Console.WriteLine("\n|---------------------------|\n" +
                              $"id: {characterData.Id}\n" +
                              $"name: {characterData.Name}\n" +
                              $"status: {characterData.Status}\n" +
                              $"species: {characterData.Species}\n" +
                              $"type: {characterData.Type}\n" +
                              $"gender: {characterData.Gender}\n" +
                              $"origin: {characterData.Origin.Name}\n" +
                              $"location: {characterData.Location.Name}\n" +
                              "episodes:\n" +
                              $"{episodeInfo}");

        }
    }

    private static async Task<string> EpisodeGetter(List<string> episodeList)
    {
        using (HttpClient cliente = new HttpClient())
        {
            string episodeinfo = "";
            foreach (var episode in episodeList)
            {
                string apiUrl = episode;
            
                // Realizar la solicitud GET a la API
                HttpResponseMessage respuesta = await cliente.GetAsync(apiUrl);

                // Leer el contenido de la respuesta como una cadena JSON
                string contenidoJson = await respuesta.Content.ReadAsStringAsync();

                // Procesar la cadena JSON for RespuestaPersonaje
                EpisodeResponse? episodeData =
                    Newtonsoft.Json.JsonConvert.DeserializeObject<EpisodeResponse>(contenidoJson);

                episodeinfo += $"  {episodeData.Episode} : {episodeData.Name}\n";
            }
            
            return episodeinfo;
        }
    }

}

// Clases para deserializar la respuesta JSON
class CharacterResponse
{

    public int Id { get; set; }
    public string Name { get; set; }
    public string Status { get; set; }
    public string Species { get; set; }
    public string Type { get; set; }
    public string Gender { get; set; }
    public OriginData  Origin { get; set; }
    public LocationData Location { get; set; }

    public List<String> Episode { get; set; }
}

class OriginData
{
    public string Name { get; set; }
}

class LocationData
{
    public string Name { get; set; }
}

class EpisodeResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Air_date { get; set; }
    public string Episode { get; set; }
}